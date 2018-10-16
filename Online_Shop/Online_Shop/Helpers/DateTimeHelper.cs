using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Helpers
{
	public static class DateTimeHelper
	{
		/// <summary>
		/// 	Zgody z epoch
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns>
		/// 	DateTime
		/// </returns>

		public static long ToTimestamp(this DateTime dateTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var time = dateTime.Subtract(new TimeSpan(epoch.Ticks));

			return (time.Ticks / 10000);
		}
	}
}
