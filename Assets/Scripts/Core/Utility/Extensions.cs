namespace Core.Utility
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    public static class Extensions
    {
		public static T SelectRandom<T>(this List<T> list, IRandom random = null)
		{
			if (list.Count == 0)
				return default(T);
			int index = random?.Range(0, list.Count) ?? Random.Range(0, list.Count);
			return list[index];
		}
		
		public static T SelectWeightedRandom<T>(this List<T> list, IRandom random = null) where T : IWeightRandomItem
		{
            if (list.Count == 0)
                return default(T);

			float sum = list.Sum(x => x.Weight);
			
			float selectedWeight = random?.Range(0,sum) ?? Random.Range(0,sum);
            
			float sreachSum =0f;
			for(int index = 0; index < list.Count; index++)
            {
				if(sreachSum + list[index].Weight > selectedWeight)
				{
					return list[index];
				}
				sreachSum += list[index].Weight;
            }

            return list[list.Count -1];
		}
	}
}