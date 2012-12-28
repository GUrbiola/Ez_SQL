using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ColorPickerLib;

namespace ColorPicker
{
	class ImagesUtil
	{
		static public ImageList GetToolbarImageList(Type type, string resourceName, Size imageSize, Color transparentColor)
		{
            System.Drawing.Bitmap bitmap = null;//new System.Drawing.Bitmap(type, resourceName);
            if (resourceName.Equals("colorbarIndicators", StringComparison.CurrentCultureIgnoreCase))
            {
                bitmap = ColorPickerResources.colorbarIndicators;
            }
            else if (resourceName.Equals("eyedropper", StringComparison.CurrentCultureIgnoreCase))
            {
                bitmap = ColorPickerResources.eyedropper;
            }
            else if (resourceName.Equals("popupcontainerbuttons", StringComparison.CurrentCultureIgnoreCase))
            {
                bitmap = ColorPickerResources.popupcontainerbuttons;
            }
			ImageList imageList = new ImageList();
			imageList.ImageSize = imageSize;
			imageList.TransparentColor = transparentColor;
			imageList.Images.AddStrip(bitmap);
			imageList.ColorDepth = ColorDepth.Depth24Bit;
			return imageList;
		}
	}

	class SelectorImages
	{
		public enum eIndexes
		{
			Right,
			Left,
			Up,
			Down, 
			Donut,
		}

		static private ImageList m_imageList = null;
		static public ImageList ImageList()
		{
			Type t = typeof(SelectorImages);
			if (m_imageList == null)
				m_imageList = ImagesUtil.GetToolbarImageList(t, "colorbarIndicators", new Size(12, 12), Color.Magenta);
			return m_imageList;
		}
		static public Image Image(eIndexes index)
		{
			return ImageList().Images[(int)index];
		}
	}
    //class PopupContainerImages
    //{
    //    public enum eIndexes
    //    {
    //        Close,
    //        Check,
    //    }

    //    static private ImageList m_imageList = null;
    //    static public ImageList ImageList()
    //    {
    //        Type t = typeof(SelectorImages);
    //        if (m_imageList == null)
    //            m_imageList = ImagesUtil.GetToolbarImageList(t, "popupcontainerbuttons", new Size(16, 16), Color.Magenta);
    //        return m_imageList;
    //    }
    //    static public Image Image(eIndexes index)
    //    {
    //        return ImageList().Images[(int)index];
    //    }
    //}
}
