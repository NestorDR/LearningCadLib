using System.Reflection;

namespace LearningCadLib
{
    internal class DxfTriangleExample : DxfTriangleExampleAssociatedHatch
    {
        internal DxfTriangleExample() : base(MethodBase.GetCurrentMethod().DeclaringType.Name.Replace("Dxf", ""), false)
        {
            // The default drawing name obtained through Reflection of the derived class name
        }
    }
}
