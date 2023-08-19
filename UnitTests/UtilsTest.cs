using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvProjectTools;

namespace UnitTests;

[TestClass]
public class UtilsTest  
{
    [TestMethod]
    public void Relative2Absolute_WorksWell()
    {
        Assert.AreEqual(
            "/home/fj/Desktop/BLECODE_Emission/sdk/app_modules/api",
            Utils.Relative2Absolute("/home/fj/Desktop/BLECODE_Emission/projects/target_apps/prod_test/prod_test/Keil_5/", @".\..\..\..\..\..\sdk\app_modules\api"));
    }
}



