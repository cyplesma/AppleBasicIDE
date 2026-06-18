using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace AppleBasic_IDE
{
    public partial class frmMain : Form
    {

        List<BasicLine> ProgramLines = new List<BasicLine>();
        private List<BasicLine> UndoProgramLines = null;
        string _currentFileName = "";
        Boolean IsDirty = false;
        bool bAllowClose = false;

        int iCurrentLineNum = -1;
        int iLastLineNumber = -1;
        int iLastReferencedLineIndex = -1;

        Boolean bLoadingFile = false;

        bool bInternalSelectionChange = false;
        bool bUpdatingHighlights = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            openFile.Filter = "Apple BASIC files (*.ab2)|*.ab2|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFile.Filter = "Apple BASIC files (*.ab2)|*.ab2|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFile.DefaultExt = "ab2";
            UpdateWindowTitle();
        }


        #region menu strip items
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbFile.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbFile.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbFile.Paste();
        }

        private void selectallStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbFile.SelectAll();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string searchText = PromptDialog.Show("Find", "Text to find:", "");
            if (string.IsNullOrEmpty(searchText))
                return;

            int start = rtbFile.SelectionStart + rtbFile.SelectionLength;
            int match = rtbFile.Find(searchText, start, RichTextBoxFinds.None);

            if (match < 0 && start > 0)
                match = rtbFile.Find(searchText, 0, RichTextBoxFinds.None);

            if (match < 0)
                MessageBox.Show($"Could not find \"{searchText}\".", "Find",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                rtbFile.Focus();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Open a new editor instance?\n\n" +
                "Yes = New instance\n" +
                "No = Start New File in this Window\n" +
                "Cancel = Continue editing",
                "New Program",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            switch (dr)
            {
                case DialogResult.Yes:
                    Process.Start(new ProcessStartInfo(Application.ExecutablePath)
                    {
                        UseShellExecute = true
                    });
                    break;

                case DialogResult.No:
                    if (!ConfirmSaveIfDirty())
                        return;
                    NewDocument();
                    break;

                case DialogResult.Cancel:
                    return;
            }

            rtbFile.Focus();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmSaveIfDirty())
                return;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFile.FileName;

                try
                {
                    LoadAppleBasicFile(fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"The file could not be opened.\n\n{ex.Message}", "Open File",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo make sure there is an actual file name and just save it. no check or confirmation to overwrite

            //todo eventually set any yellow flagged lines to be green
            if (string.IsNullOrWhiteSpace(_currentFileName))
            {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }

            SaveAppleBasicFile(_currentFileName);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo open file dialog and allow person to select a folder and enter a file name to save the file. if the file exists confirm to overwrite it (might be part of save process already

            //todo eventually set any yellow flagged lines to be green

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                SaveAppleBasicFile(saveFile.FileName);
            }
        }

        private void closefileStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConfirmSaveIfDirty())
                NewDocument();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmOptions f = new frmOptions())
                f.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAbout f = new frmAbout())
                f.ShowDialog();
        }

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncProgramLinesFromEditor();
            lstIssues.Items.Clear();

            foreach (BasicLine line in ProgramLines)
                line.Issues.Clear();

            AnalyzeGotoReferences();

            AnalyzeShortHandGotoReferences();

            AnalyzeGosubReferences();

            AnalyzeDuplicateLineNumbers();
            AnalyzeInvalidLines();

            //AnalyzeUnusedSubroutines();

            tsfIssueCount.Text = $"Issues: {lstIssues.Items.Count}";


            if (lstIssues.Items.Count == 0)
            {
                MessageBox.Show("No Issues Found", "All Good", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tsfIssueCount.BackColor = Color.LightGreen;
            }
            else
            {
                tsfIssueCount.BackColor = Color.LightPink;
            }

            RepaintEditorHighlights();
        }

        private void renumberToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SyncProgramLinesFromEditor();

            if (ProgramLines.GroupBy(line => line.LineNumber)
                .Any(group => group.Key.HasValue && group.Count() > 1))
            {
                MessageBox.Show("Duplicate line numbers must be fixed before renumbering.",
                    "Renumber", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(PromptDialog.Show("Renumber", "Starting line number:", "10"), out int startNumber)
                || startNumber < 0)
                return;

            if (!int.TryParse(PromptDialog.Show("Renumber", "Line number increment:", "10"), out int increment)
                || increment <= 0)
                return;

            List<RenumberLines> renumList =
                BuildRenumberList(ProgramLines, startNumber, increment);

            Dictionary<int, int> lineMap =
                BuildLineNumberLookup(renumList);

            FillGotoGosubReferences(ProgramLines, renumList, lineMap);

            ApplyRenumberingToProgramLines(ProgramLines, renumList, lineMap);

            RebuildEditorFromProgramLines();
            IsDirty = true;
            UpdateWindowTitle();
        }

        #endregion

        private void NewDocument()
        {
            bLoadingFile = true;
            rtbFile.Clear();
            ProgramLines.Clear();
            if ((UndoProgramLines) != null)
            {
                UndoProgramLines.Clear();
            }

            iCurrentLineNum = -1;
            iLastLineNumber = -1;
            iLastReferencedLineIndex = -1;

            bLoadingFile = true;

            bInternalSelectionChange = false;

            _currentFileName = string.Empty;

            IsDirty = false;
            lstIssues.Items.Clear();
            tsfIssueCount.Text = "Issues: 0";
            tsfIssueCount.BackColor = Color.White;
            bLoadingFile = false;
            UpdateWindowTitle();
        }

        private bool LoadAppleBasicFile(string fileName)
        {
            bLoadingFile = true;
            string[] fileLines = File.ReadAllLines(fileName);

            if (fileLines.Length == 0)
            {
                bLoadingFile = false;
                MessageBox.Show("File is empty.");
                return false;
            }

            int startLine = 0;

            if (fileLines.Length > 0 &&
                fileLines[0].Trim().Equals("APPLEIIBASIC", StringComparison.OrdinalIgnoreCase))
            {
                startLine = 1;
            }

            rtbFile.Clear();
            rtbFile.Lines = fileLines.Skip(startLine).ToArray();

            bLoadingFile = false;
            SyncProgramLinesFromEditor();
            RepaintEditorHighlights();
            _currentFileName = fileName;
            IsDirty = false;
            UpdateWindowTitle();
            UpdateCursorPosition();
            return true;
        }

        private bool SaveAppleBasicFile(string fileName)
        {
            try
            {
                List<string> lines = new List<string> { "APPLEIIBASIC" };
                lines.AddRange(rtbFile.Lines);
                File.WriteAllLines(fileName, lines);

                _currentFileName = fileName;
                IsDirty = false;
                UpdateWindowTitle();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The file could not be saved.\n\n{ex.Message}", "Save File",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void rtbFile_TextChanged(object sender, EventArgs e)
        {
            // RichTextBox formatting changes can raise TextChanged. Do not rebuild
            // ProgramLines while an internal highlighting pass is enumerating it.
            if (bLoadingFile || bInternalSelectionChange || bUpdatingHighlights)
                return;

            SyncProgramLinesFromEditor();
            IsDirty = true;
            UpdateWindowTitle();
            tlsfNumberOfLines.Text = " Total Lines: " + ProgramLines.Count;
        }

        private void rtbFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 'a' || e.KeyChar > 'z')
                return;

            int lineIndex = rtbFile.GetLineFromCharIndex(rtbFile.SelectionStart);
            int lineStart = rtbFile.GetFirstCharIndexFromLine(lineIndex);
            if (lineStart < 0)
                return;

            int prefixLength = Math.Max(0, rtbFile.SelectionStart - lineStart);
            string linePrefix = rtbFile.Text.Substring(lineStart, prefixLength);

            if (!IsInsideQuotedString(linePrefix) || ContainsRemKeywordOutsideQuotes(linePrefix))
                e.KeyChar = char.ToUpperInvariant(e.KeyChar);
        }

        private bool IsInsideQuotedString(string text)
        {
            bool insideQuotes = false;

            foreach (char character in text)
            {
                if (character == '"')
                    insideQuotes = !insideQuotes;
            }

            return insideQuotes;
        }

        private bool ContainsRemKeywordOutsideQuotes(string text)
        {
            bool insideQuotes = false;

            for (int i = 0; i <= text.Length - 3; i++)
            {
                if (text[i] == '"')
                {
                    insideQuotes = !insideQuotes;
                    continue;
                }

                if (insideQuotes)
                    continue;

                if (string.Compare(text, i, "REM", 0, 3, StringComparison.OrdinalIgnoreCase) != 0)
                    continue;

                bool startsAtBoundary = i == 0 || !char.IsLetterOrDigit(text[i - 1]);
                bool endsAtBoundary = i + 3 == text.Length || !char.IsLetterOrDigit(text[i + 3]);

                if (startsAtBoundary && endsAtBoundary)
                    return true;
            }

            return false;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bAllowClose)
                return;

            if (!ConfirmSaveIfDirty())
            {
                e.Cancel = true;
                return;
            }

            bAllowClose = true;
        }

        private bool ConfirmSaveIfDirty()
        {
            if (!IsDirty)
                return true;

            DialogResult result = MessageBox.Show(
                "Save changes to the current BASIC program?",
                "Unsaved Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
                return false;

            if (result == DialogResult.No)
                return true;

            if (string.IsNullOrWhiteSpace(_currentFileName))
            {
                if (saveFile.ShowDialog() != DialogResult.OK)
                    return false;

                return SaveAppleBasicFile(saveFile.FileName);
            }

            return SaveAppleBasicFile(_currentFileName);
        }

        private void UpdateWindowTitle()
        {
            string name = string.IsNullOrWhiteSpace(_currentFileName)
                ? "Untitled"
                : Path.GetFileName(_currentFileName);
            Text = $"Apple BASIC IDE - {name}{(IsDirty ? " *" : "")}";
        }

        private void SyncProgramLinesFromEditor()
        {
            ProgramLines.Clear();

            string[] editorLines = rtbFile.Lines;
            int count = editorLines.Length;
            if (count > 0 && editorLines[count - 1].Length == 0)
                count--;

            for (int i = 0; i < count; i++)
                ProgramLines.Add(ParseBasicLine(editorLines[i], i));
        }

        private BasicLine ParseBasicLine(string text, int editorLineIndex)
        {
            Match lineMatch = Regex.Match(text ?? "", @"^\s*(\d+)(?:\s+(.*))?$");
            int? lineNumber = null;
            string codeOnly = "";

            if (lineMatch.Success && int.TryParse(lineMatch.Groups[1].Value, out int parsedNumber))
            {
                lineNumber = parsedNumber;
                codeOnly = lineMatch.Groups[2].Success ? lineMatch.Groups[2].Value : "";
            }

            Match remMatch = Regex.Match(text ?? "", @"\bREM\b", RegexOptions.IgnoreCase);

            return new BasicLine
            {
                EditorLineIndex = editorLineIndex,
                LineNumber = lineNumber,
                Text = text ?? "",
                CodeOnly = codeOnly,
                iRemStartIndex = remMatch.Success ? remMatch.Index : -1
            };
        }

        private void UpdateCursorPosition()
        {
            if (bUpdatingHighlights)
                return;

            bUpdatingHighlights = true;

            try
            {
                int iOldStart = rtbFile.SelectionStart;
                int iOldLength = rtbFile.SelectionLength;

                int index = rtbFile.SelectionStart;
                int line = rtbFile.GetLineFromCharIndex(index);

                if (!bLoadingFile)
                {
                    if (iLastLineNumber >= ProgramLines.Count)
                        iLastLineNumber = -1;

                    if (iLastReferencedLineIndex >= ProgramLines.Count)
                        iLastReferencedLineIndex = -1;

                    if (iLastLineNumber >= 0 && iLastLineNumber != line)
                        PaintLine(iLastLineNumber);

                    if (line >= 0 && line < ProgramLines.Count)
                    {
                        iCurrentLineNum = line;
                        HighlightCurrentLine(iCurrentLineNum);

                        HighlightReferencedLineFromCursor();

                        iLastLineNumber = line;
                    }
                }

                int lineStart = rtbFile.GetFirstCharIndexFromLine(line);
                int column = lineStart >= 0 ? index - lineStart : 0;

                tlsfNumberOfLines.Text = " Total Lines: " + rtbFile.Lines.Length.ToString();
                tsfCurrentLineNumber.Text = " Current Line: " + (line + 1).ToString();
                tsfCurrentLinePosition.Text = " Current Position: " + (column + 1).ToString();

                if ((column + 1) > 90)
                    tsfCurrentLinePosition.BackColor = Color.Red;
                else if ((column + 1) > 70)
                    tsfCurrentLinePosition.BackColor = Color.Yellow;
                else
                    tsfCurrentLinePosition.BackColor = Color.White;

                rtbFile.Select(iOldStart, iOldLength);
            }
            finally
            {
                bUpdatingHighlights = false;
            }
        }

        private void rtbFile_SelectionChanged(object sender, EventArgs e)
        {
            if (bInternalSelectionChange || bUpdatingHighlights)
                return;

            UpdateCursorPosition();
        }



        #region editor highlighting, etc
        private void AddBasicLineToEditor(BasicLine basicLine)
        {

            int iLineStart = rtbFile.TextLength;

            rtbFile.AppendText(basicLine.Text + Environment.NewLine);

                if (basicLine.iRemStartIndex >= 0)
                {
                    int iHighlightStart = iLineStart + basicLine.iRemStartIndex;
                    int iHighlightLength = Math.Max(0,
                        Math.Min(basicLine.Text.Length - basicLine.iRemStartIndex,
                            rtbFile.TextLength - iHighlightStart));

                    if (iHighlightStart >= 0 && iHighlightStart <= rtbFile.TextLength)
                    {
                        rtbFile.Select(iHighlightStart, iHighlightLength);
                        rtbFile.SelectionBackColor = Color.LightGreen;
                    }
                }

            // restore cursor to end
            rtbFile.Select(rtbFile.TextLength, 0);
            rtbFile.SelectionBackColor = rtbFile.BackColor;
        }

        private void PaintLine(int iLine)
        {
            if (iLine < 0 || iLine >= ProgramLines.Count || bLoadingFile == true) //TODO add flag to not run this when a new program is being loaded in same window
                return;

            bInternalSelectionChange = true;

            try
            {
                int iOldStart = rtbFile.SelectionStart;
                int iOldLength = rtbFile.SelectionLength;

                BasicLine line = ProgramLines[iLine];

                if (!TryGetEditorLineRange(iLine, out int iLineStart, out int iLineLength))
                    return;

                line.eHighlightType = LineHighlightType.Normal;

                rtbFile.Select(iLineStart, iLineLength);
                rtbFile.SelectionBackColor = GetColorPerHighLightType(line.eHighlightType);

                if (line.iRemStartIndex >= 0)
                {
                    int commentStart = iLineStart + line.iRemStartIndex;
                    int commentLength = Math.Max(0, iLineLength - line.iRemStartIndex);
                    if (commentStart >= 0 && commentStart <= rtbFile.TextLength)
                    {
                        rtbFile.Select(commentStart, commentLength);
                        rtbFile.SelectionBackColor = Color.LightGreen;
                    }
                }

                rtbFile.Select(iOldStart, iOldLength);
            }
            finally
            {
                bInternalSelectionChange = false;
            }
        }

        private void HighlightCurrentLine(int iLine)
        {
            if (iLine < 0 || iLine >= ProgramLines.Count)
                return;

            bInternalSelectionChange = true;

            try
            {
                int iOldStart = rtbFile.SelectionStart;
                int iOldLength = rtbFile.SelectionLength;

                if (!TryGetEditorLineRange(iLine, out int iLineStart, out int iLineLength))
                    return;

                rtbFile.Select(iLineStart, iLineLength);
                ProgramLines[iLine].eHighlightType = LineHighlightType.CurrentLine;
                rtbFile.SelectionBackColor = GetColorPerHighLightType(ProgramLines[iLine].eHighlightType);

                rtbFile.Select(iOldStart, iOldLength);
            }
            finally
            {
                bInternalSelectionChange = false;
            }
        }

        private void HighlightLineBackColor(int iLine, LineHighlightType eHighLightToUse)
        {
            if (iLine < 0 || iLine >= ProgramLines.Count)
                return;

            if (ProgramLines[iLine].eHighlightType == eHighLightToUse)
                return;

            bInternalSelectionChange = true;

            try
            {
                int iOldStart = rtbFile.SelectionStart;
                int iOldLength = rtbFile.SelectionLength;

                if (!TryGetEditorLineRange(iLine, out int iLineStart, out int iLineLength))
                    return;

                ProgramLines[iLine].eHighlightType = eHighLightToUse;

                rtbFile.Select(iLineStart, iLineLength);
                rtbFile.SelectionBackColor = GetColorPerHighLightType(eHighLightToUse);

                rtbFile.Select(iOldStart, iOldLength);
            }
            finally
            {
                bInternalSelectionChange = false;
            }
        }

        private int? GetTargetLineNumberAtColumn(string text, int column)
        {
            if (string.IsNullOrEmpty(text) || column < 0)
                return null;

            // Check ON...GOTO/GOSUB lists first. The generic GOTO pattern below
            // only sees the first number in one of these lists.
            foreach (Match onMatch in Regex.Matches(
                text,
                @"\bON\b.+?\b(?:GOTO|GOSUB)\b\s+(?<targets>\d+(?:\s*,\s*\d+)+)",
                RegexOptions.IgnoreCase))
            {
                Group targets = onMatch.Groups["targets"];

                foreach (Match numberMatch in Regex.Matches(targets.Value, @"\d+"))
                {
                    int numberStart = targets.Index + numberMatch.Index;
                    int numberEnd = numberStart + numberMatch.Length;

                    if (column >= numberStart && column < numberEnd
                        && int.TryParse(numberMatch.Value, out int target))
                    {
                        return target;
                    }
                }
            }

            MatchCollection matches = Regex.Matches(
                text,
                @"\b(GOTO|GOSUB|THEN)\s+(\d+)",
                RegexOptions.IgnoreCase);

            foreach (Match m in matches)
            {
                Group g = m.Groups[2];

                if (column >= g.Index && column < g.Index + g.Length)
                {
                    if (int.TryParse(g.Value, out int target))
                        return target;
                }
            }

            return null;
        }

        private void HighlightReferencedLineFromCursor()
        {


            int iCursorLine = rtbFile.GetLineFromCharIndex(rtbFile.SelectionStart);
            if (iCursorLine < 0 || iCursorLine >= ProgramLines.Count)
                return;

            int cursorLineStart = rtbFile.GetFirstCharIndexFromLine(iCursorLine);
            if (cursorLineStart < 0)
                return;

            int iCursorColumn = rtbFile.SelectionStart - cursorLineStart;

            BasicLine currentLine = ProgramLines[iCursorLine];

            int? targetLineNumber = GetTargetLineNumberAtColumn(currentLine.Text, iCursorColumn);

            // remove old reference highlight
            if (iLastReferencedLineIndex >= 0)
            {
                PaintLine(iLastReferencedLineIndex);
                iLastReferencedLineIndex = -1;
            }

            if (!targetLineNumber.HasValue)
                return;

            int targetIndex = ProgramLines.FindIndex(x =>
                x.LineNumber.HasValue &&
                x.LineNumber.Value == targetLineNumber.Value);

            if (targetIndex < 0)
                return;

            HighlightLineBackColor(targetIndex, LineHighlightType.ReferencedLine);
            ProgramLines[targetIndex].eHighlightType = LineHighlightType.ReferencedLine;
            iLastReferencedLineIndex = targetIndex;
        }



        private Color GetColorPerHighLightType(LineHighlightType eHighLightToUse)
        {
            switch (eHighLightToUse)
            {
                case LineHighlightType.CurrentLine:
                    return Color.LightYellow;

                case LineHighlightType.ReferencedLine:
                    return Color.LightBlue;

                case LineHighlightType.ErrorLine:
                    return Color.LightPink;

                case LineHighlightType.ExecutionLine:
                    return Color.LightCoral;

                default:
                    return Color.White;
            }
        }

        #endregion


        #region analyze stuff
        private void AnalyzeGotoReferences()
        {
            HashSet<int> existingLineNumbers = new HashSet<int>();

            foreach (BasicLine line in ProgramLines)
            {
                if (line.LineNumber.HasValue)
                    existingLineNumbers.Add(line.LineNumber.Value);
            }

            foreach (BasicLine line in ProgramLines)
            {
                foreach (int targetLine in FindGotoTargets(line.CodeOnly))
                {
                    if (!existingLineNumbers.Contains(targetLine))
                    {
                        string msg = $"GOTO target {targetLine} does not exist";

                        line.Issues.Add(msg);

                        lstIssues.Items.Add(new BasicIssue
                        {
                            EditorLineIndex = line.EditorLineIndex,
                            LineNumber = line.LineNumber,
                            Message = msg
                        });
                    }
                }
            }
        }

        private List<int> FindGotoTargets(string code)
        {
            List<int> targets = new List<int>();

            if (string.IsNullOrWhiteSpace(code))
                return targets;

            // Ignore anything after REM for now
            int remIndex = code.IndexOf("REM", StringComparison.OrdinalIgnoreCase);
            if (remIndex >= 0)
                code = code.Substring(0, remIndex);

            MatchCollection matches = Regex.Matches(
                code,
                @"\bGOTO\s+(\d+)",
                RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int lineNumber))
                    targets.Add(lineNumber);
            }

            return targets;
        }

        private void AnalyzeShortHandGotoReferences()
        {
            HashSet<int> existingLineNumbers = new HashSet<int>();

            foreach (BasicLine line in ProgramLines)
            {
                if (line.LineNumber.HasValue)
                    existingLineNumbers.Add(line.LineNumber.Value);
            }

            foreach (BasicLine line in ProgramLines)
            {
                foreach (int targetLine in FindShorthandGotoTargets(line.CodeOnly))
                {
                    if (!existingLineNumbers.Contains(targetLine))
                    {
                        string msg = $"GOTO target {targetLine} does not exist";

                        line.Issues.Add(msg);

                        lstIssues.Items.Add(new BasicIssue
                        {
                            EditorLineIndex = line.EditorLineIndex,
                            LineNumber = line.LineNumber,
                            Message = msg
                        });
                    }
                }
            }
        }

        private List<int> FindShorthandGotoTargets(string code)
        {
            List<int> targets = new List<int>();

            if (string.IsNullOrWhiteSpace(code))
                return targets;

            // Ignore anything after REM for now
            int remIndex = code.IndexOf("REM", StringComparison.OrdinalIgnoreCase);
            if (remIndex >= 0)
                code = code.Substring(0, remIndex);

            MatchCollection matches = Regex.Matches(
                code,
                @"\bTHEN\s+(\d+)",
                RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int lineNumber))
                    targets.Add(lineNumber);
            }

            return targets;
        }

        private void AnalyzeGosubReferences()
        {
            HashSet<int> existingLineNumbers = new HashSet<int>();

            foreach (BasicLine line in ProgramLines)
            {
                if (line.LineNumber.HasValue)
                    existingLineNumbers.Add(line.LineNumber.Value);
            }

            foreach (BasicLine line in ProgramLines)
            {
                foreach (int targetLine in FindGosubTargets(line.CodeOnly))
                {
                    if (!existingLineNumbers.Contains(targetLine))
                    {
                        string msg = $"GOSUB target {targetLine} does not exist";

                        line.Issues.Add(msg);

                        lstIssues.Items.Add(new BasicIssue
                        {
                            EditorLineIndex = line.EditorLineIndex,
                            LineNumber = line.LineNumber,
                            Message = msg
                        });
                    }
                }
            }
        }

        private List<int> FindGosubTargets(string code)
        {
            List<int> targets = new List<int>();

            if (string.IsNullOrWhiteSpace(code))
                return targets;

            // Ignore anything after REM for now
            int remIndex = code.IndexOf("REM", StringComparison.OrdinalIgnoreCase);
            if (remIndex >= 0)
                code = code.Substring(0, remIndex);

            MatchCollection matches = Regex.Matches(
                code,
                @"\bGOSUB\s+(\d+)",
                RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int lineNumber))
                    targets.Add(lineNumber);
            }

            return targets;
        }

        private void AnalyzeDuplicateLineNumbers()
        {
            Dictionary<int, int> lineCounts = new Dictionary<int, int>();

            // Count occurrences
            foreach (BasicLine line in ProgramLines)
            {
                if (!line.LineNumber.HasValue)
                    continue;

                int lineNumber = line.LineNumber.Value;

                if (lineCounts.ContainsKey(lineNumber))
                    lineCounts[lineNumber]++;
                else
                    lineCounts.Add(lineNumber, 1);
            }

            // Find duplicates
            foreach (BasicLine line in ProgramLines)
            {
                if (!line.LineNumber.HasValue)
                    continue;

                int lineNumber = line.LineNumber.Value;

                if (lineCounts[lineNumber] > 1)
                {
                    string msg = $"Duplicate line number {lineNumber}";

                    line.Issues.Add(msg);

                    lstIssues.Items.Add(new BasicIssue()
                    {
                        EditorLineIndex = line.EditorLineIndex,
                        LineNumber = line.LineNumber,
                        Message = msg
                    });
                }
            }
        }

        private void AnalyzeInvalidLines()
        {
            foreach (BasicLine line in ProgramLines)
            {
                if (line.LineNumber.HasValue)
                    continue;

                string message = string.IsNullOrWhiteSpace(line.Text)
                    ? "Blank program line"
                    : "Line must begin with a BASIC line number";

                line.Issues.Add(message);
                lstIssues.Items.Add(new BasicIssue
                {
                    EditorLineIndex = line.EditorLineIndex,
                    LineNumber = null,
                    Message = message
                });
            }
        }
        #endregion

        #region renumber
    //    private List<RenumberLines> BuildRenumberList(
    //List<BasicLine> programLines,
    //int startNumber,
    //int increment)
    //    {
    //        List<RenumberLines> renumList = new List<RenumberLines>();

    //        int newLineNumber = startNumber;

    //        for (int i = 0; i < programLines.Count; i++)
    //        {
    //            BasicLine bl = programLines[i];

    //            if (bl.LineNumber.HasValue)
    //            {
    //                renumList.Add(new RenumberLines
    //                {
    //                    EditorLineIndex = bl.EditorLineIndex,
    //                    iOriginalLine = bl.LineNumber.Value,
    //                    iNewLine = newLineNumber
    //                });

    //                newLineNumber += increment;
    //            }
    //        }

    //        return renumList;
    //    }

        private List<RenumberLines> BuildRenumberList(
            List<BasicLine> programLines,
            int startNumber,
            int increment)
        {
            List<RenumberLines> renumList = new List<RenumberLines>();

            int newLineNumber = startNumber;

            for (int i = 0; i < programLines.Count; i++)
            {
                BasicLine bl = programLines[i];

                if (bl.LineNumber.HasValue)
                {
                    renumList.Add(new RenumberLines
                    {
                        EditorLineIndex = bl.EditorLineIndex,
                        iOriginalLine = bl.LineNumber.Value,
                        iNewLine = newLineNumber
                    });

                    newLineNumber += increment;
                }
            }

            return renumList;
        }

        //private Dictionary<int, int> BuildLineNumberLookup(List<RenumberLines> renumList)
        //{
        //    return renumList.ToDictionary(x => x.iOriginalLine, x => x.iNewLine);
        //}

        private Dictionary<int, int> BuildLineNumberLookup(List<RenumberLines> renumList)
        {
            return renumList.ToDictionary(x => x.iOriginalLine, x => x.iNewLine);
        }

        //    private void FillGotoGosubReferences(
        //List<BasicLine> programLines,
        //List<RenumberLines> renumList,
        //Dictionary<int, int> lineMap)
        //    {
        //        foreach (RenumberLines rn in renumList)
        //        {
        //            BasicLine bl = programLines.FirstOrDefault(x => x.EditorLineIndex == rn.EditorLineIndex);
        //            if (bl == null)
        //                continue;

        //            string code = bl.CodeOnly;
        //            rn.iOriginalList.Clear();
        //            rn.iNewList.Clear();

        //            // GOTO 100 / GOSUB 200
        //            foreach (Match m in Regex.Matches(
        //                code,
        //                @"\b(GOTO|GOSUB)\s+(\d+)",
        //                RegexOptions.IgnoreCase))
        //            {
        //                AddReference(rn, lineMap, m.Groups[2].Value);
        //            }

        //            // THEN 100 / THEN GOTO 100 / THEN GOSUB 100
        //            foreach (Match m in Regex.Matches(
        //                code,
        //                @"\bTHEN\s+(?:(?:GOTO|GOSUB)\s+)?(\d+)",
        //                RegexOptions.IgnoreCase))
        //            {
        //                AddReference(rn, lineMap, m.Groups[1].Value);
        //            }

        //            // ON X GOTO 100,200,300
        //            // ON X GOSUB 1000,2000,3000
        //            foreach (Match m in Regex.Matches(
        //                code,
        //                @"\bON\b.+?\b(GOTO|GOSUB)\b\s+([0-9,\s]+)",
        //                RegexOptions.IgnoreCase))
        //            {
        //                string[] nums = m.Groups[2].Value.Split(',');

        //                foreach (string num in nums)
        //                {
        //                    AddReference(rn, lineMap, num.Trim());
        //                }
        //            }
        //        }
        //    }


        private void FillGotoGosubReferences(
            List<BasicLine> programLines,
            List<RenumberLines> renumList,
            Dictionary<int, int> lineMap)
        {
            foreach (RenumberLines rn in renumList)
            {
                BasicLine bl = programLines.FirstOrDefault(x => x.EditorLineIndex == rn.EditorLineIndex);
                if (bl == null)
                    continue;

                string code = bl.CodeOnly;
                rn.iOriginalList.Clear();
                rn.iNewList.Clear();

                // GOTO 100 / GOSUB 200
                foreach (Match m in Regex.Matches(
                    code,
                    @"\b(GOTO|GOSUB)\s+(\d+)",
                    RegexOptions.IgnoreCase))
                {
                    AddReference(rn, lineMap, m.Groups[2].Value);
                }

                // THEN 100 / THEN GOTO 100 / THEN GOSUB 100
                foreach (Match m in Regex.Matches(
                    code,
                    @"\bTHEN\s+(?:(?:GOTO|GOSUB)\s+)?(\d+)",
                    RegexOptions.IgnoreCase))
                {
                    AddReference(rn, lineMap, m.Groups[1].Value);
                }

                // ON X GOTO 100,200,300
                // ON X GOSUB 1000,2000,3000
                foreach (Match m in Regex.Matches(
                    code,
                    @"\bON\b.+?\b(GOTO|GOSUB)\b\s+([0-9,\s]+)",
                    RegexOptions.IgnoreCase))
                {
                    string[] nums = m.Groups[2].Value.Split(',');

                    foreach (string num in nums)
                    {
                        AddReference(rn, lineMap, num.Trim());
                    }
                }
            }
        }


        //    private void AddReference(
        //RenumberLines rn,
        //Dictionary<int, int> lineMap,
        //string value)
        //    {
        //        if (!int.TryParse(value, out int oldNumber))
        //            return;

        //        rn.iOriginalList.Add(oldNumber);

        //        if (lineMap.TryGetValue(oldNumber, out int newNumber))
        //            rn.iNewList.Add(newNumber);
        //        else
        //            rn.iNewList.Add(oldNumber); // missing target, leave unchanged for now
        //    }


        private void AddReference(
    RenumberLines rn,
    Dictionary<int, int> lineMap,
    string value)
        {
            if (!int.TryParse(value, out int oldNumber))
                return;

            rn.iOriginalList.Add(oldNumber);

            if (lineMap.TryGetValue(oldNumber, out int newNumber))
                rn.iNewList.Add(newNumber);
            else
                rn.iNewList.Add(oldNumber); // missing target, leave unchanged for now
        }


        //    private void ApplyRenumberingToProgramLines(
        //List<BasicLine> programLines,
        //List<RenumberLines> renumList,
        //Dictionary<int, int> lineMap)
        //    {
        //        foreach (RenumberLines rn in renumList)
        //        {
        //            BasicLine bl = programLines.FirstOrDefault(x => x.EditorLineIndex == rn.EditorLineIndex);
        //            if (bl == null)
        //                continue;

        //            string text = bl.Text;

        //            // Replace leading BASIC line number
        //            text = Regex.Replace(
        //                text,
        //                @"^\s*\d+",
        //                rn.iNewLine.ToString());

        //            // Replace GOTO/GOSUB targets
        //            text = Regex.Replace(
        //                text,
        //                @"\b(GOTO|GOSUB)\s+(\d+)",
        //                m => ReplaceSingleTarget(m, lineMap),
        //                RegexOptions.IgnoreCase);

        //            // Replace THEN targets
        //            text = Regex.Replace(
        //                text,
        //                @"\bTHEN\s+(?:(GOTO|GOSUB)\s+)?(\d+)",
        //                m => ReplaceThenTarget(m, lineMap),
        //                RegexOptions.IgnoreCase);

        //            // Replace ON X GOTO/GOSUB target lists
        //            text = Regex.Replace(
        //                text,
        //                @"\bON\b(.+?)\b(GOTO|GOSUB)\b\s+([0-9,\s]+)",
        //                m => ReplaceOnGotoList(m, lineMap),
        //                RegexOptions.IgnoreCase);

        //            bl.Text = text;
        //            bl.LineNumber = rn.iNewLine;
        //        }
        //    }

        private void ApplyRenumberingToProgramLines(
    List<BasicLine> programLines,
    List<RenumberLines> renumList,
    Dictionary<int, int> lineMap)
        {
            foreach (RenumberLines rn in renumList)
            {
                BasicLine bl = programLines.FirstOrDefault(x => x.EditorLineIndex == rn.EditorLineIndex);
                if (bl == null)
                    continue;

                string text = bl.Text;

                // Replace leading BASIC line number
                text = Regex.Replace(
                    text,
                    @"^\s*\d+",
                    rn.iNewLine.ToString());

                // Replace each branch expression exactly once. ON...GOTO and
                // THEN alternatives come first so their nested GOTO keywords
                // are not also processed by the generic GOTO/GOSUB branch.
                text = Regex.Replace(
                    text,
                    @"\bON\b(?<onExpression>.+?)\b(?<onKeyword>GOTO|GOSUB)\b\s+(?<onTargets>\d+(?:\s*,\s*\d+)+)|" +
                    @"\bTHEN\s+(?:(?<thenKeyword>GOTO|GOSUB)\s+)?(?<thenTarget>\d+)|" +
                    @"\b(?<singleKeyword>GOTO|GOSUB)\s+(?<singleTarget>\d+)",
                    m => ReplaceBranchReference(m, lineMap),
                    RegexOptions.IgnoreCase);

                bl.Text = text;
                bl.LineNumber = rn.iNewLine;
            }
        }



        //private string ReplaceSingleTarget(Match m, Dictionary<int, int> lineMap)
        //{
        //    string keyword = m.Groups[1].Value;
        //    int oldTarget = int.Parse(m.Groups[2].Value);

        //    int newTarget = lineMap.ContainsKey(oldTarget)
        //        ? lineMap[oldTarget]
        //        : oldTarget;

        //    return keyword + " " + newTarget;
        //}

        //private string ReplaceThenTarget(Match m, Dictionary<int, int> lineMap)
        //{
        //    string thenPart = m.Groups[0].Value;
        //    string optionalKeyword = m.Groups[1].Value;
        //    int oldTarget = int.Parse(m.Groups[2].Value);

        //    int newTarget = lineMap.ContainsKey(oldTarget)
        //        ? lineMap[oldTarget]
        //        : oldTarget;

        //    if (string.IsNullOrWhiteSpace(optionalKeyword))
        //        return "THEN " + newTarget;

        //    return "THEN " + optionalKeyword + " " + newTarget;
        //}
        //private string ReplaceOnGotoList(Match m, Dictionary<int, int> lineMap)
        //{
        //    string expressionPart = m.Groups[1].Value;
        //    string keyword = m.Groups[2].Value;
        //    string listPart = m.Groups[3].Value;

        //    string[] nums = listPart.Split(',');

        //    for (int i = 0; i < nums.Length; i++)
        //    {
        //        string trimmed = nums[i].Trim();

        //        if (int.TryParse(trimmed, out int oldTarget))
        //        {
        //            int newTarget = lineMap.ContainsKey(oldTarget)
        //                ? lineMap[oldTarget]
        //                : oldTarget;

        //            nums[i] = newTarget.ToString();
        //        }
        //    }

        //    return "ON" + expressionPart + keyword + " " + string.Join(",", nums);
        //}

        private string ReplaceBranchReference(Match match, Dictionary<int, int> lineMap)
        {
            if (match.Groups["onTargets"].Success)
            {
                string targets = Regex.Replace(
                    match.Groups["onTargets"].Value,
                    @"\d+",
                    number => MapLineNumber(number.Value, lineMap));

                return "ON" + match.Groups["onExpression"].Value
                    + match.Groups["onKeyword"].Value + " " + targets;
            }

            if (match.Groups["thenTarget"].Success)
            {
                string keyword = match.Groups["thenKeyword"].Value;
                string mappedTarget = MapLineNumber(
                    match.Groups["thenTarget"].Value, lineMap);

                return string.IsNullOrWhiteSpace(keyword)
                    ? "THEN " + mappedTarget
                    : "THEN " + keyword + " " + mappedTarget;
            }

            return match.Groups["singleKeyword"].Value + " "
                + MapLineNumber(match.Groups["singleTarget"].Value, lineMap);
        }

        private string MapLineNumber(string value, Dictionary<int, int> lineMap)
        {
            if (!int.TryParse(value, out int oldTarget))
                return value;

            return lineMap.TryGetValue(oldTarget, out int newTarget)
                ? newTarget.ToString()
                : oldTarget.ToString();
        }

        private void RenumberProgram(int startNumber, int increment)
        {
            List<RenumberLines> renumList =
                BuildRenumberList(ProgramLines, startNumber, increment);

            Dictionary<int, int> lineMap =
                BuildLineNumberLookup(renumList);

            FillGotoGosubReferences(ProgramLines, renumList, lineMap);

            ApplyRenumberingToProgramLines(ProgramLines, renumList, lineMap);

            RebuildEditorFromProgramLines();
        }

        //private void RebuildEditorFromProgramLines()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (BasicLine bl in ProgramLines)
        //    {
        //        sb.AppendLine(bl.Text);
        //    }

        //    rtbFile.Text = sb.ToString();

        //    // TODO Then rerun your existing coloring/analyze pass
        //    //AnalyzeProgram();
        //    //RepaintEditorHighlights();
        //}


        private void RebuildEditorFromProgramLines()
        {
            StringBuilder sb = new StringBuilder();

            foreach (BasicLine bl in ProgramLines)
            {
                sb.AppendLine(bl.Text);
            }

            bLoadingFile = true;
            rtbFile.Text = sb.ToString().TrimEnd('\r', '\n');
            bLoadingFile = false;
            SyncProgramLinesFromEditor();

            // Then rerun your existing coloring/analyze pass
            analyzeToolStripMenuItem_Click(this,null);
            RepaintEditorHighlights();
        }

        private void RepaintEditorHighlights()
        {
            if (ProgramLines.Count == 0)
                return;

            bInternalSelectionChange = true;
            try
            {
                int oldStart = rtbFile.SelectionStart;
                int oldLength = rtbFile.SelectionLength;

                rtbFile.SelectAll();
                rtbFile.SelectionBackColor = rtbFile.BackColor;

                // Use a snapshot as a second line of defense against WinForms
                // events that may run while RichTextBox formatting is applied.
                foreach (BasicLine line in ProgramLines.ToList())
                {
                    int lineStart = rtbFile.GetFirstCharIndexFromLine(line.EditorLineIndex);
                    if (lineStart < 0)
                        continue;

                    if (line.Issues.Count > 0)
                    {
                        int lineLength = Math.Min(
                            line.Text.Length,
                            Math.Max(0, rtbFile.TextLength - lineStart));
                        rtbFile.Select(lineStart, lineLength);
                        rtbFile.SelectionBackColor = Color.LightPink;
                    }

                    if (line.iRemStartIndex >= 0)
                    {
                        int commentStart = lineStart + line.iRemStartIndex;
                        int commentLength = Math.Max(0,
                            Math.Min(line.Text.Length - line.iRemStartIndex,
                                rtbFile.TextLength - commentStart));

                        if (commentStart >= 0 && commentStart <= rtbFile.TextLength)
                        {
                            rtbFile.Select(commentStart, commentLength);
                            rtbFile.SelectionBackColor = Color.LightGreen;
                        }
                    }
                }

                int restoredStart = Math.Min(oldStart, rtbFile.TextLength);
                rtbFile.Select(restoredStart,
                    Math.Min(oldLength, Math.Max(0, rtbFile.TextLength - restoredStart)));
            }
            finally
            {
                bInternalSelectionChange = false;
            }
        }

        private bool TryGetEditorLineRange(int lineIndex, out int start, out int length)
        {
            start = -1;
            length = 0;

            if (lineIndex < 0 || lineIndex >= ProgramLines.Count)
                return false;

            start = rtbFile.GetFirstCharIndexFromLine(lineIndex);
            if (start < 0 || start > rtbFile.TextLength)
                return false;

            length = Math.Min(
                ProgramLines[lineIndex].Text.Length,
                Math.Max(0, rtbFile.TextLength - start));

            return true;
        }

        private void lstIssues_DoubleClick(object sender, EventArgs e)
        {
            if (!(lstIssues.SelectedItem is BasicIssue issue))
                return;

            int lineIndex = issue.EditorLineIndex;
            if (lineIndex < 0 || lineIndex >= rtbFile.Lines.Length)
                return;

            int start = rtbFile.GetFirstCharIndexFromLine(lineIndex);
            if (start < 0)
                return;

            rtbFile.Select(start, rtbFile.Lines[lineIndex].Length);
            rtbFile.Focus();
        }
        #endregion


    }
}
