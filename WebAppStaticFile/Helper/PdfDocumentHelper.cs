using IronSoftware.Drawing;

namespace WebAppStaticFile.Helper;

public static class PdfDocumentHelper
{
    public static void DrawSignature(PdfDocument doc,int page, string signature, int x, int y, int width, int height)
    {
        string base64String = signature.Substring("data:image/png;base64,".Length);
        byte[] image = Convert.FromBase64String(base64String);
        AnyBitmap bitmap = new AnyBitmap(image);
        doc.DrawBitmap(bitmap,page, x, y, width, height);
    }
}