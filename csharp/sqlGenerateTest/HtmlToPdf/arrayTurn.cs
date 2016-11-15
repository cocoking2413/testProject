using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdf {
	public static class arrayTurn {
		public static double [ , ] translate(this double [ , ] arr, double x, double y) {
			double [ , ] turn = new double [arr.Length / 2, 2];
			if ( arr.Length > 0 ) {
				for ( int i = 0; i < arr.Length / 2; i++ ) {
					turn [i, 0] = arr [i, 0] + x;
					turn [i, 1] = arr [i, 1] + y;
				}
				return turn;
			}
			return arr;
		}
		public static double [ , ] scale(this double [ , ] arr, double x, double y) {
			double [ , ] turn = new double [arr.Length / 2, 2];
			if ( arr.Length > 0 ) {
				for ( int i = 0; i < arr.Length / 2; i++ ) {
					turn [i, 0] = arr [i, 0] * x;
					turn [i, 1] = arr [i, 1] * y;
				}
				return turn;
			}
			return arr;
		}
		public static double [ , ] rotate(this double [ , ] arr, int r) {
			double [ , ] turn = new double [arr.Length / 2, 2];
			if ( arr.Length > 0 ) {
				for ( int i = 0; i < arr.Length / 2; i++ ) {
					turn [i, 0] = arr [i, 0] * Math.Cos(r * Math.PI / 180) - arr [i, 1] * Math.Sin(r * Math.PI / 180);
					turn [i, 1] = arr [i, 0] * Math.Sin(r * Math.PI / 180) + arr [i, 1] * Math.Cos(r * Math.PI / 180);
				}
				return turn;
			}
			return arr;
		}
        public static int rad(this double[,] arr,bool left=true) {
            return 0;
        }
	}
}
