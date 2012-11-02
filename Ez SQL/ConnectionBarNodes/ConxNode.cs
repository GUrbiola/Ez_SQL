using System;
using System.Collections.Generic;
using AddressBarExt;
using System.Drawing;
using Ez_SQL.ConnectionManagement;


namespace Ez_SQL.ConnectionBarNodes
{
	/// <summary>
	/// Description of ConxNode.
	/// </summary>
	public class ConxNode : AddressBarExt.IAddressNode
	{

        #region Class Variables
		ConnectionInfo ThisConx;
        
        
        /// <summary>
        /// Stores the parent node to this node
        /// </summary>
        private IAddressNode parent = null;

        /// <summary>
        /// Stores the display name of this Node
        /// </summary>
        private String szDisplayName = null;

        /// <summary>
        /// Stores the Icon for this node
        /// </summary>
        private Icon icon = null;

        /// <summary>
        /// Stores the child nodes
        /// </summary>
        private IAddressNode[] children = null;

        /// <summary>
        /// Stores user defined data for this node
        /// </summary>
        private Object tag = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the parent node to this node
        /// </summary>
        public IAddressNode Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        /// <summary>
        /// Gets/Sets the Display name of this node
        /// </summary>
        public String DisplayName
        {
            get{return this.szDisplayName;}
            set { this.szDisplayName = value; }
        }

        /// <summary>
        /// Gets the Icon that represents this node type.
        /// </summary>
        public Icon Icon
        {
            get { return Globals.MakeIcon(Properties.Resources.Database, 22, false); }
        }

        /// <summary>
        /// Returns the Unique Id for this node
        /// </summary>
        public Object UniqueID
        {
            get 
            {
                if (ThisConx != null)
                {
                    return String.Format("{0}|Name:{1}|ConnectionString:{2}",Parent.UniqueID,DisplayName,ThisConx.ConnectionString);
                }
            	return "";
            }
        }

        /// <summary>
        /// Gets/Sets user defined data for this object
        /// </summary>
        public Object Tag
        {
            get { return this.tag; }
            set { this.tag = value; }
        }

        /// <summary>
        /// Gets the children of this node
        /// </summary>
        public IAddressNode[] Children
        {
            get { return this.children; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Basic Constructor, initializes this node to start at the root of the first drive found on the disk. ONLY USE THIS FOR ROOT NODES
        /// </summary>
        public ConxNode(ConxGroupNode Dad, ConnectionInfo Conx)
        {
        	parent = Dad;
        	szDisplayName = Conx.Name;
        	ThisConx = Conx;
        }

        #endregion

        #region Destructor

        ~ConxNode()
        {
        }

        #endregion

        #region Node Update

        /// <summary>
        /// Updates the contents of this node.
        /// </summary>
        public void UpdateNode()
        {
        }

        #endregion

        #region General

        /// <summary>
        /// Returns an individual child node, based on a given unique ID. NOT IMPLEMENTED.
        /// </summary>
        /// <param name="uniqueID">Unique Object to identify the child</param>
        /// <param name="recursive">Indicates whether we should recursively search child nodes</param>
        /// <returns>Returns a child node. Returns null if method fails.</returns>
        public IAddressNode GetChild(object uniqueID, bool recursive)
        {
            return null;
        }

        /// <summary>
        /// Creates a clone of this node
        /// </summary>
        /// <returns>Cloned Node</returns>
        public IAddressNode Clone()
        {
        	//return new ConxNode(parent, ThisConx);
        	return null;
        }

        /// <summary>
        /// Populates this node as a root node representing "My Computer"
        /// </summary>
        private void GenerateRootNode()
        {
            UpdateNode();

		    //get the icon
            GenerateNodeDisplayDetails();
        }

        /// <summary>
        /// Sets the icon for the given path
        /// </summary>
        private void GenerateNodeDisplayDetails()
        {
        	return;
//            //if the path exists
//            if (icon == null)
//            {
//                //needed to get a handle to our icon
//                SHFILEINFO shinfo = new SHFILEINFO();
//
//                //If we have an actual path, then we pass a string
//                if (fullPath.Length > 0)
//                {
//                    //get the icon and display name
//                    Win32.SHGetFileInfo(fullPath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON | Win32.SHGFI_DISPLAYNAME);
//                }
//                else
//                {
//                    //If we get a blank path we assume the root of our file system, so we get a pidl to My "Computer"
//
//                    //Get a pidl to my computer
//                    IntPtr tempPidl = System.IntPtr.Zero;
//                    Win32.SHGetSpecialFolderLocation(0, Win32.CSIDL_DRIVES, ref tempPidl);
//
//                    //get the icon and display name
//                    Win32.SHGetFileInfo(tempPidl, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_PIDL | Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON | Win32.SHGFI_DISPLAYNAME);
//
//                    //free our pidl
//                    Marshal.FreeCoTaskMem(tempPidl);
//                }
//
//                //create the managed icon
//                this.icon =(Icon)System.Drawing.Icon.FromHandle(shinfo.hIcon).Clone();
//                this.szDisplayName = shinfo.szDisplayName;
//
//                //dispose of the old icon
//                Win32.DestroyIcon(shinfo.hIcon);
//            }
        }

        #endregion
	}
}
