using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace CodeCoverage
{
    public class FilteredFileNameEditor : UITypeEditor
    {
        private OpenFileDialog ofd = new OpenFileDialog();

        public override UITypeEditorEditStyle GetEditStyle(
         ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(
         ITypeDescriptorContext context,
         IServiceProvider provider,
         object value)
        {
            ofd.FileName = Convert.ToString(value);
            ofd.Filter = "Assemblies|*.dll; *.exe";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }
            return base.EditValue(context, provider, value);
        }
    }

}
