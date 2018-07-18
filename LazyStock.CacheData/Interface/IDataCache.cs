using System;
using System.Collections.Generic;
using System.Text;

namespace LazyStock.CacheData.Interface
{
    interface IDataCache
    {
        void Query();
        void Update();
        void Delete();
        void Insert();
        void Reload();
    }
}
