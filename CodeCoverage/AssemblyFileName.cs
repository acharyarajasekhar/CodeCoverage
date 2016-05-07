using System.ComponentModel;

namespace CodeCoverage
{
    public class AssemblyFileName
    {
        [Category("File")]
        [Description("Assembly file for Code Coverage")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
