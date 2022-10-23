using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dapper;

namespace Discount.Grpc.Configurations
{
    public sealed class ColumnMapper<TEntity> where TEntity : class
    {
        private readonly Dictionary<string, string> _columnMapping;
        private ColumnMapper()
        {
            _columnMapping = new Dictionary<string, string>();
        }

        public static ColumnMapper<TEntity> CreateColumnMap()
        {
            return new ColumnMapper<TEntity>();
        }

        public ColumnMapper<TEntity> SetMap(Expression<Func<TEntity, object>> columnExpression, string dbColumName)
        {
            if (_columnMapping.ContainsKey(dbColumName)) return this;
            if (columnExpression.Body is not MemberExpression body)
            {
                var ubody = (UnaryExpression)columnExpression.Body;
                body = ubody.Operand as MemberExpression;
            }

            _columnMapping.Add(dbColumName, body.Member.Name);

            return this;
        }

        public void Map()
        {
            SqlMapper.SetTypeMap(typeof(TEntity), new CustomPropertyTypeMap(typeof(TEntity), (type, columnName) => type.GetProperty(_columnMapping[columnName])));
        }
    }
}
