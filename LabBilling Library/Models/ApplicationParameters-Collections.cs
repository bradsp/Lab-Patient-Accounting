using System;
using System.ComponentModel;
using Utilities;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Collections Category

    private System.String _collectionsFileLocation;
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsFileLocation 
    { 
        get => _collectionsFileLocation.ToGenericFilePath(); 
        set => _collectionsFileLocation = value; 
    }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpPassword { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpServer { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpUsername { get; set; }


    private System.String _collectionsSftpUploadPath;
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String CollectionsSftpUploadPath 
    { 
        get => _collectionsSftpUploadPath.ToGenericFilePath(); 
        set => _collectionsSftpUploadPath = value; 
    }

    private System.String _statementsFileLocation;
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsFileLocation 
    { 
        get => _statementsFileLocation.ToGenericFilePath(); 
        set => _statementsFileLocation = value; 
    }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpPassword { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpServer { get; set; }
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpUsername { get; set; }

    private System.String _statementsSftpUploadPath;
    [Category(_collectionsCategory)]
    [Description("")]
    public System.String StatementsSftpUploadPath 
    { 
        get => _statementsSftpUploadPath.ToGenericFilePath(); 
        set => _statementsSftpUploadPath = value; 
    }
    [Category(_collectionsCategory)]
    [Description("Number of statements sent before sending account to collections.")]
    public Int32 NumberOfStatementsBeforeCollection { get; set; }
    #endregion Collections Category
}
