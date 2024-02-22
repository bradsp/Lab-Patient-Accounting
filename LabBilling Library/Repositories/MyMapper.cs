using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class MyMapper : PetaPoco.IMapper
    {
        private PetaPoco.StandardMapper standardMapper = new PetaPoco.StandardMapper();
        public PetaPoco.TableInfo GetTableInfo(Type pocoType)
        {
            return standardMapper.GetTableInfo(pocoType);
        }
        public PetaPoco.ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
        {
            return standardMapper.GetColumnInfo(pocoProperty);
        }
        public Func<object, object> GetFromDbConverter(PropertyInfo TargetProperty, Type SourceType)
        {
            if (TargetProperty.PropertyType == typeof(string))
            {
                return (x) => x.ToString();
            }
            return standardMapper.GetFromDbConverter(TargetProperty, SourceType);
        }
        public Func<object, object> GetToDbConverter(PropertyInfo SourceProperty)
        {
            if (SourceProperty.PropertyType == typeof(string))
            {
                return (x) => x != null ? x.ToAnsiString() : "".ToAnsiString();
            }
            return standardMapper.GetToDbConverter(SourceProperty);
        }
    }
}
