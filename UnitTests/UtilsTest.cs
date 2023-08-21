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

    [TestMethod]
    [DataRow("no", "yes", "", 0)]
    [DataRow("abcdef", "abcdef", "abcdef", 6)]
    [DataRow("abcdefg", "abcdefh", "abcdef", 6)]
    public void CommonStart_WorksWell(string s1, string s2, string expectedCommon, int expectedMaxCommonIndex)
    {
        string foundCommon = Utils.CommonStart(s1, s2, out int foundCommonIndex);
        Assert.AreEqual(expectedCommon, foundCommon);
        Assert.AreEqual(expectedMaxCommonIndex, foundCommonIndex);
    }

    [TestMethod]
    [DataRow( new string[] { "no", "yes" }, "")]
    [DataRow( new string[] { "mammaa", "mammab", "mammaabc", "mamma123"}, "mamma")]
    [DataRow( new string[] { "mamma", "mamma", "mamma", "mamma"}, "mamma")]
    public void FindCommonStart_WorksWell(string[] testStrings, string expectedCommon)
    {
        string foundCommon = Utils.FindCommonStart(testStrings);
        Assert.AreEqual(expectedCommon, foundCommon);
    }

    
    [TestMethod]
    [DataRow (
        @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot\boot.h",
        @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot\ARM_clang\boot.h",
        @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot")]
    public void FindCommontPath_WorksWell(string path1, string path2, string expectedCommonPath)
    {
        string foundCommonPath = Utils.FindCommonPath(path1, path2);
        Assert.AreEqual(expectedCommonPath, foundCommonPath);
    }

    [TestMethod]
    [DataRow (
        new string[] {
            @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot\boot.h",
            @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot\ARM\boot.h",
            @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot\ARM_clang\boot.h",
            @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot\GCC\boot.h",
            @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot\IAR\boot.h"            
        },
        @"C:\Workspace\BLECODE_Emission\sdk\platform\arch\boot")]

    public void FindCommonBase_WorksWell(string[] paths, string expectedBase)
    {
        string foundBase = Utils.FindCommonBase(paths);
        Assert.AreEqual(expectedBase, foundBase);
    }
}
