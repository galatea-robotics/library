namespace System.Drawing
{
    public sealed partial class Graphics
    {
        internal Image Image
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }
    }
}