using System;
using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Collections Category
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsFileLocation { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpPassword { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpServer { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpUsername { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpUploadPath { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsFileLocation { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpPassword { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpServer { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpUsername { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpUploadPath { get; set; }
    [Category(_collectionsCategory)]
    [Description("Number of statements sent before sending account to collections.")]
    public Int32 NumberOfStatementsBeforeCollection { get; set; }
    #endregion Collections Category
}
