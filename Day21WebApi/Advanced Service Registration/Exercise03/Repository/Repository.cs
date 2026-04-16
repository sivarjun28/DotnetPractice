namespace Exercise03.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _data = new();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.FromResult(_data.AsEnumerable());
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var prop = typeof(T).GetProperty("Id");
            var entity = _data.FirstOrDefault(x => prop != null && (int)prop.GetValue(x)! == id);
            return await Task.FromResult(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            _data.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            var prop = typeof(T).GetProperty("Id");
            if (prop == null) return await Task.FromResult<T?>(null);

            var id = (int)prop.GetValue(entity)!;
            var existing = _data.FirstOrDefault(x => (int)prop.GetValue(x)! == id);

            if (existing == null) return await Task.FromResult<T?>(null);

            _data.Remove(existing);
            _data.Add(entity);

            return await Task.FromResult(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var prop = typeof(T).GetProperty("Id");
            if (prop == null) return await Task.FromResult(false);

            var entity = _data.FirstOrDefault(x => (int)prop.GetValue(x)! == id);
            if (entity == null) return await Task.FromResult(false);

            _data.Remove(entity);
            return await Task.FromResult(true);
        }
    }
}