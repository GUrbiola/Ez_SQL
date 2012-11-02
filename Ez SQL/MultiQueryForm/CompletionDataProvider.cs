using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;
using ICSharpCode.TextEditor;

namespace Ez_SQL.MultiQueryForm
{
    public enum FilteringType { Table, View, Procedure, ScalarFunction, TableFunction, FieldItem, ScriptItem, Any, Smart }
    public class CompletionDataProvider: ICompletionDataProvider
    {
        private ImageList imageList;
        SQLConnector SqlServerData;
        public FilteringType FilteringOption { get; set; }
        private string FSchema, FObject, FChild;
        public string FilterString 
        { 
            get 
            {
                if (FSchema == null)
                {
                    if(FChild == null)
                        return FObject; 
                    else
                        return String.Format("{0}.{1}", FObject, FChild);
                }
                else
                {
                    if(FChild == null)
                        return String.Format("{0}.{1}", FSchema, FObject);
                    else
                        return String.Format("{0}.{1}.{2}", FSchema, FObject, FChild);
                }
            } 
        }
        public int FilteringLevel
        {
            get
            {
                if (FSchema == null)
                {
                    if (FChild == null)
                        return 1;
                    else
                        return 2;
                }
                else
                {
                    if (FChild == null)
                        return 3;
                    else
                        return 4;
                }
            } 
        }
        public List<ISqlObject> ComplementaryObjects{ get; set; }

        
        public CompletionDataProvider(SQLConnector SqlServerData, ImageList imageList)
        {
            this.SqlServerData = SqlServerData;
            this.imageList = imageList;
            ComplementaryObjects = new List<ISqlObject>();
            FilteringOption = FilteringType.Any;
        }
        public CompletionDataProvider(SQLConnector SqlServerData, ImageList imageList, string FSchema, string FObject, string FChild)
        {
            this.SqlServerData = SqlServerData;
            this.imageList = imageList;
            this.FSchema = FSchema;
            this.FObject = FObject;
            this.FChild = FChild;
            ComplementaryObjects = new List<ISqlObject>();
            FilteringOption = FilteringType.Any;
        }
        public ImageList ImageList { get { return imageList; } }
        public string PreSelection { get { return null; } }
        public int DefaultIndex { get { return -1; } }
        private bool _IsEmpty;
        public bool IsEmpty { get { return _IsEmpty; } }
        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_')
            {
                return CompletionDataProviderKeyResult.NormalKey;
            }
            return CompletionDataProviderKeyResult.InsertionKey;
        }
        /// <summary>
        /// Called when entry should be inserted. Forward to the insertion action of the completion data.
        /// </summary>
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(
                Math.Min(insertionOffset, textArea.Document.TextLength)
                );
            return data.InsertAction(textArea, key);
        }
        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            //List<ISqlObject> BackItems = new List<ISqlObject>();
            List<ICompletionData> BackItems = new List<ICompletionData>();
            
            switch (FilteringOption)
            {
                case FilteringType.Table:
                    BackItems.AddRange(ComplementaryObjects);
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Kind == ObjectType.Table && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case FilteringType.View:
                    BackItems.AddRange(ComplementaryObjects);
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Kind == ObjectType.View && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case FilteringType.Procedure:
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Kind == ObjectType.Procedure && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case FilteringType.ScalarFunction:
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Kind == ObjectType.ScalarFunction && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case FilteringType.TableFunction:
                    BackItems.AddRange(ComplementaryObjects);
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Kind == ObjectType.TableFunction && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case FilteringType.FieldItem:
                    BackItems.AddRange(ComplementaryObjects);
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => (X.Kind == ObjectType.TableFunction || X.Kind == ObjectType.View || X.Kind == ObjectType.Table) && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case FilteringType.ScriptItem:
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => (X.Kind != ObjectType.Table && X.Kind != ObjectType.Schema) && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case FilteringType.Smart:
                    switch (FilteringLevel)
	                {
                        case 1://only 1 item received for autocomplete(FObject)
                        default:
                            BackItems.AddRange(ComplementaryObjects.Where(X => X.Name.StartsWith(FObject, StringComparison.CurrentCultureIgnoreCase)));
                            BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Name.StartsWith(FObject, StringComparison.CurrentCultureIgnoreCase)));
                            break;
                        case 2://two items received for autocomplete(FObject, FChild)
                            List<ISqlObject> Buffer = SqlServerData.DbObjects.Where(X => X.Name.Equals(FObject, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            Buffer.AddRange(ComplementaryObjects.Where(X => X.Name.Equals(FObject, StringComparison.CurrentCultureIgnoreCase)));
                            foreach (ISqlObject Parent in Buffer)
	                        {
                                BackItems.AddRange(Parent.Childs.Where(X => X.Name.StartsWith(FChild, StringComparison.CurrentCultureIgnoreCase) && X.Kind == ChildType.Field));
	                        }
                            break;
                        case 3://two items received for autocomplete(FSchema, FObject)
                            BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Schema.Equals(FSchema, StringComparison.CurrentCultureIgnoreCase) && X.Name.StartsWith(FObject, StringComparison.CurrentCultureIgnoreCase) && X.Kind != ObjectType.Schema));
                            break;
                        case 4://three items received for autocomplete(FSchema, FObject, FChild)
                            List<ISqlObject> Buffer2 = SqlServerData.DbObjects.Where(X => X.Schema.Equals(FSchema, StringComparison.CurrentCultureIgnoreCase) && X.Name.Equals(FObject, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            foreach (ISqlObject Parent in Buffer2)
	                        {
                                BackItems.AddRange(Parent.Childs.Where(X => X.Name.StartsWith(FChild, StringComparison.CurrentCultureIgnoreCase)));
	                        }
                            break;
	                }
                    break;
                case FilteringType.Any:
                default:
                    BackItems.AddRange(ComplementaryObjects);
                    BackItems.AddRange(SqlServerData.DbObjects.Where(X => X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
                    break;
            }

            BackItems.Sort((X, Y) => String.Compare(X.Text, Y.Text));

            return BackItems.ToArray();

        }
    }

}
