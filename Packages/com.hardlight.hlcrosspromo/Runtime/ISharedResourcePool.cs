
namespace Hardlight
{
	// Original HLCrossPromo.Runtime 02000024; bodies translated from the 1.10.1 x86-64 binary.
	public interface ISharedResourcePool<TKey, TResource>
	{
		bool TryGet(TKey key, out TResource resource);

		void Add(TKey key, TResource resource);

		void ReleaseResource(TResource texture);
	}
}
