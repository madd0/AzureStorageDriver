//-----------------------------------------------------------------------
// <copyright file="ExtendedTableQuery.cs" company="madd0.com">
//     Copyright (c) 2014 Mauricio DIAZ ORLICH.
//     Code licensed under the MIT X11 license.
// </copyright>
// <author>Mauricio DIAZ ORLICH</author>
//-----------------------------------------------------------------------

namespace Madd0.UserQuery
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class ExtendedTableQuery
    {

        public TextWriter QueryWriter
        {
            get;
            set;
        }

    }
    public class ExtendedTableQuery<TElement> : ExtendedTableQuery, IEnumerable, IEnumerable<TElement>, IQueryable, IQueryable<TElement>
        where TElement : ITableEntity, new()
    {
        private CloudTable _table;
        private TableQuery<TElement> _query;

        public ExtendedTableQuery(CloudTable table)
        {
            _table = table;
            _query = table.CreateQuery<TElement>();
        }

        public ICancellableAsyncResult BeginExecuteSegmented(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, System.AsyncCallback callback, object state)
        {
            return _query.BeginExecuteSegmented(currentToken, requestOptions, operationContext, callback, state);
        }

        public ICancellableAsyncResult BeginExecuteSegmented(TableContinuationToken currentToken, System.AsyncCallback callback, object state)
        {
            return _query.BeginExecuteSegmented(currentToken, callback, state);
        }

        public TableQuerySegment<TElement> EndExecuteSegmented(System.IAsyncResult asyncResult)
        {
            return _query.EndExecuteSegmented(asyncResult);
        }

        public System.Collections.Generic.IEnumerable<TElement> Execute(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
        {
            return _query.Execute(requestOptions, operationContext);
        }

        public TableQuerySegment<TElement> ExecuteSegmented(TableContinuationToken continuationToken, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
        {
            return _query.ExecuteSegmented(continuationToken, requestOptions, operationContext);
        }

        public System.Threading.Tasks.Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, System.Threading.CancellationToken cancellationToken)
        {
            return _query.ExecuteSegmentedAsync(currentToken, requestOptions, operationContext, cancellationToken);
        }

        public System.Threading.Tasks.Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext)
        {
            return _query.ExecuteSegmentedAsync(currentToken, requestOptions, operationContext);
        }

        public System.Threading.Tasks.Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, System.Threading.CancellationToken cancellationToken)
        {
            return _query.ExecuteSegmentedAsync(currentToken, cancellationToken);
        }

        public System.Threading.Tasks.Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken)
        {
            return _query.ExecuteSegmentedAsync(currentToken);
        }

        public System.Collections.Generic.IEnumerator<TElement> GetEnumerator()
        {
            return _query.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public TableQuery<TElement> Select(System.Collections.Generic.IList<string> columns)
        {
            return _query.Select(columns);
        }

        public TableQuery<TElement> Take(int? take)
        {
            return _query.Take(take);
        }

        public TableQuery<TElement> Where(string filter)
        {
            return _query.Where(filter);
        }

        public System.Type ElementType
        {
            get
            {
                return _query.ElementType;
            }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get
            {
                return _query.Expression;
            }
        }

        public string FilterString
        {
            set
            {
                _query.FilterString = value;
            }
            get
            {
                return _query.FilterString;
            }
        }

        public System.Linq.IQueryProvider Provider
        {
            get
            {
                return _query.Provider;
            }
        }

        public System.Collections.Generic.IList<string> SelectColumns
        {
            set
            {
                _query.SelectColumns = value;
            }
            get
            {
                return _query.SelectColumns;
            }
        }

        public int? TakeCount
        {
            set
            {
                _query.TakeCount = value;
            }
            get
            {
                return _query.TakeCount;
            }
        }

        public CloudTable CloudTable
        {
            get
            {
                return _table;
            }
        }
    }
}
