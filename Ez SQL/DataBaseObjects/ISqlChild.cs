using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace Ez_SQL.DataBaseObjects
{
    public interface ISqlChild : ICompletionData
    {
        int Id { get; set; }
        string Name { get; set; }
        ChildType Kind { get; }
        string Type { get; set; }
        int Precision { get; set; }
        bool Nullable { get; set; }
        bool Computed { get; set; }
        bool IsPrimaryKey { get; set; }
        int Seed { get; set; }
        int Increment { get; set; }
        bool IsIdentity { get; set; }
        string IdentityScript { get; }
        string Comment { get; set; }
        ISqlObject Parent { get; set; }
        bool IsForeignKey { get; }
        int ForeignKey { get; set; }
        string DefaultValue { get; set; }
        ISqlObject ReferenceParent { get; set; }
        string ReferenceParentName { get; set; }
        ISqlChild ReferenceChild { get; set; }
        string ReferenceChildName { get; set; }
    }
}
