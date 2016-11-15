using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DBContact
{
	/// <summary>
	/// DAL基类，实现Repository通用泛型数据访问模式
	/// </summary>
	public class baseDBContext : DbContext, IDisposable {
		public baseDBContext(string connectionString):base(connectionString) {
            if (connectionString.IndexOf("name=") != 0)
            {
                this.Database.Connection.ConnectionString = connectionString;
                this.Configuration.LazyLoadingEnabled = false;
                this.Configuration.ProxyCreationEnabled = false;
            }
		}


		public T Update<T>(T entity) where T : class {
			var set = this.Set<T>();
			set.Attach(entity);
			this.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;

			int result = this.SaveChanges();
			if ( result <= 0 ) return null;

			return entity;
		}


		public T Insert<T>(T entity) where T : class {
            try
            {
                this.Set<T>().Add(entity);

                int result = this.SaveChanges();
                if (result <= 0) return null;

            }
            catch(Exception ex) { }
            return entity;

		}

		public void Delete<T>(T entity) where T : class {
			this.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
			this.SaveChanges();
		}

		public T Find<T>(params object [ ] keyValues) where T : class {
			return this.Set<T>().Find(keyValues);
		}

		public List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : class {
			if ( conditions == null )
				return this.Set<T>().ToList();
			else
				return this.Set<T>().Where(conditions).ToList();
		}
		/// <summary>
		/// EF SQL 语句返回 dataTable
		/// </summary>
		/// <param name="db"></param>
		/// <param name="sql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DataTable SqlQueryForDataTatable(string sql) {
			Database db = this.Database;
			SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			conn = ( SqlConnection ) db.Connection;
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = sql;

			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			DataTable table = new DataTable();
			adapter.Fill(table);

			conn.Close();//连接需要关闭
			conn.Dispose();
			return table;
		}

	}
}
