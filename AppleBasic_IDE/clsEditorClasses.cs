using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppleBasic_IDE
{
    public class BasicLine
    {
        public int EditorLineIndex { get; set; }
        public int? LineNumber { get; set; }
        public string Text { get; set; } = "";
        public string CodeOnly { get; set; } = "";

        public int iRemStartIndex = -1;

        public bool IsNew { get; set; }
        public bool IsChanged { get; set; }
        public bool IsSaved { get; set; }

        public LineHighlightType eHighlightType { get; set; }

        public bool IsCurrentExecutionLine { get; set; }

        public List<string> Issues { get; set; } = new List<string>();
    }


    public class BasicIssue
    {
        public int EditorLineIndex { get; set; }
        public int? LineNumber { get; set; }
        public string Message { get; set; } = "";

        public override string ToString()
        {
            string lineText = LineNumber.HasValue
                ? $"Line {LineNumber.Value}"
                : $"Editor Line {EditorLineIndex + 1}";

            return $"{lineText}: {Message}";
        }
    }


    public class LineRange
    {
        public string Name { get; set; } = "";
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public int Step { get; set; } = 10;
    }

    public class RenumberLines
    {
        public int EditorLineIndex { get; set; }
        public int iOriginalLine { get; set; }
        public int iNewLine { get; set; }

        // GOTO / GOSUB / THEN / ON...GOTO target numbers
        public List<int> iOriginalList { get; set; } = new List<int>();
        public List<int> iNewList { get; set; } = new List<int>();
    }

    public enum LineStatus
    {
        Normal,
        New,
        Changed,
        Saved,
        Error,
        Warning
    }

    public enum FileLoadStatus
    {
        Unknown,
        PlainText,
        AppleBasicProject,
        MissingHeader,
        HasWarnings,
        HasErrors
    }

    public enum LineHighlightType
    {
        Normal,
        CurrentLine,
        ReferencedLine,
        ErrorLine,
        ExecutionLine
    }

}
