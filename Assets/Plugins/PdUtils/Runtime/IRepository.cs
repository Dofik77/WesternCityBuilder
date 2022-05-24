using System.Collections.Generic;

namespace PdUtils
{
	public interface IRepository<TKey, TItem>
	{
		IEnumerable<TItem> GetAllItems();
		
		void Add(TKey id, TItem item);
		TItem Get(TKey id);
		bool HasItem(TKey id);
		void Update(TKey id, TItem item);
		void Delete(TKey id);
	}
}