
namespace CodeCoverage.Properties
{
    internal sealed partial class Settings
    {
        private Settings():base()
        {
            if(this["ListOfAssemblies"] == null)
            {
                ListOfAssemblies = new System.Collections.Generic.List<CodeCoverage.AssemblyFileName>();
            }
        }
    }
}
