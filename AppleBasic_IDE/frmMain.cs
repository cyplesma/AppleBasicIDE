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
        private string UndoEditorText = "";
        private int UndoSelectionStart = 0;
        string _currentFileName = "";
        Boolean IsDirty = false;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = "Apple Basic IDE - (No File)";


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
                    Process.Start(Application.ExecutablePath);
                    break;

                case DialogResult.No:
                    //if (!ConfirmSaveIfDirty())
                    //    return;

                    NewDocument();
                    this.Text = "AppleBasic IDE - " + "{(New_Program)}";
                    break;

                case DialogResult.Cancel:
                    return;
            }

            this.Text = "Apple Basic IDE - (No File)";

            rtbFile.Focus();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO open file dialog for text files *.TXT, and *.AB2
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFile.FileName;

                MessageBox.Show(fileName);

                //rtbFile.Text = File.ReadAllText(fileName);
                if (LoadAppleBasicFile(fileName) == true)
                {
                    this.Text = "AppleBasic IDE - " + "{(" + fileName +")}";


                }
            }

            //When a valid .AB@ file is selected (first line will have APPLEIIBASIC on the very first line, that will not be shown in the rtf, consider having a status , hass issues, is good to load, anything else
            //When .txt is shown first line will still be chacked for APPLEIIBASIC and will not be shown in the rtf text box, but no validation will be done

            //OriginalText = File.ReadAllText(filename);
            //CurrentText = rtbCode.Text;
            //ProgramLines = ParseBasicLines(CurrentText);


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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo check if the file "isDirty", if so confirm to save it
            //todo open file dialog as save As and allow person to save before closing program

            Application.Exit();
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

        #endregion

        private void NewDocument()
        {
            rtbFile.Clear();

            _currentFileName = string.Empty;

            this.Text = "Apple BASIC IDE";

            IsDirty = false;
        }

        private bool LoadAppleBasicFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            string[] fileLines = File.ReadAllLines(fileName);

            if (lines.Length == 0)
            {
                MessageBox.Show("File is empty.");
                return false;
            }

            bool hasAppleBasicHeader =
                lines[0].Trim().ToUpper() == "APPLEIIBASIC";

            // Remove header from editor display
            string textToDisplay ="";

            ProgramLines.Clear();


            int startLine = 0;

            if (fileLines.Length > 0 &&
                fileLines[0].Trim().ToUpper() == "APPLEIIBASIC")
            {
                startLine = 1;
            }
            int iEditorLine = 1;
            int iCodeLineID = -1;

            for (int i = startLine; i < fileLines.Length; i++)
            {
                if (fileLines[i].Trim().Length > 1)
                {
                    string sCodeLineID = fileLines[i].Trim().Substring(0, fileLines[i].IndexOf(' '));
                    string sCodeOnly = fileLines[i].Trim().Substring(fileLines[i].IndexOf(' '));
                    iCodeLineID = int.Parse(sCodeLineID);
                    ProgramLines.Add(new BasicLine()
                    {
                        CodeOnly = sCodeOnly,
                        EditorLineIndex = iEditorLine,
                        LineNumber = iCodeLineID,
                        Text = fileLines[i]
                    });
                }
                else
                {
                    ProgramLines.Add(new BasicLine()
                    {
                        CodeOnly = "",
                        EditorLineIndex = iEditorLine,
                        LineNumber = -1,
                        Text = "????????"
                    }) ;

                    lstIssues.Items.Add("Editor Line: " + iEditorLine.ToString() + " is blank");
                    tsfIssueCount.Text = $"Issues: {lstIssues.Items.Count}";

                }
                iEditorLine ++;

                textToDisplay = textToDisplay + fileLines[i] + Environment.NewLine;
            }

            rtbFile.Text = textToDisplay;


            //if (hasAppleBasicHeader)
            //{


            //    textToDisplay = string.Join(
            //        Environment.NewLine,
            //        lines.Skip(1));
            //}
            //else
            //{
            //    textToDisplay = string.Join(
            //        Environment.NewLine,
            //        lines);
            //}

            //rtbFile.Text = textToDisplay;

            //string extension = Path.GetExtension(fileName).ToUpper();

            //if (extension == ".AB2")
            //{
            //    if (!hasAppleBasicHeader)
            //    {
            //        lstIssues.Items.Add("Invalid .AB2 file - APPLEIIBASIC header missing");
            //        return false;
            //    }

            //    lstIssues.Items.Add("Apple BASIC Project Loaded");
            //    //AnalyzeProgram();
            //}
            //else if (extension == ".TXT")
            //{
            //    if (hasAppleBasicHeader)
            //        lstIssues.Items.Add("Text file with Apple BASIC header");
            //    else
            //        lstIssues.Items.Add("Plain text file loaded");

            //    // No validation
            //}

            return true;
        }

        private void SaveAppleBasicFile(string fileName)
        {
            List<string> lines = new List<string>();

            lines.Add("APPLEIIBASIC");

            lines.AddRange(
                rtbFile.Lines);

            File.WriteAllLines(fileName, lines);

            _currentFileName = fileName;
        }

        private void rtbFile_LocationChanged(object sender, EventArgs e)
        {
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            int index = rtbFile.SelectionStart;

            int iTotalLines = rtbFile.Lines.Length;

            int line = rtbFile.GetLineFromCharIndex(index);

            int column =
                index - rtbFile.GetFirstCharIndexFromLine(line);

            tlsfNumberOfLines.Text = " Total Lines: " + iTotalLines.ToString();

            tsfCurrentLineNumber.Text = " Current Line: " + (line + 1).ToString();

            tsfCurrentLinePosition.Text = " Current Position: " + (column + 1).ToString();
            if ((column + 1) > 90)
            {
                tsfCurrentLinePosition.BackColor = Color.Red;
            }
            else if ((column + 1) > 70)
            {
                tsfCurrentLinePosition.BackColor = Color.Yellow;
            }
            else
            {
                tsfCurrentLinePosition.BackColor = Color.White;
            }

        }

        private void rtbFile_SelectionChanged(object sender, EventArgs e)
        {
            UpdateCursorPosition();
        }

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo determine steps in analyze process then add to the class below and then add to lstStatus
            lstIssues.Items.Clear();

            foreach (BasicLine line in ProgramLines)
                line.Issues.Clear();

            AnalyzeGotoReferences();

            AnalyzeShortHandGotoReferences();

            AnalyzeGosubReferences();

            AnalyzeDuplicateLineNumbers();

            //AnalyzeUnusedSubroutines();

            tsfIssueCount.Text = $"Issues: {lstIssues.Items.Count}";
        }


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
        #endregion
    }
}
