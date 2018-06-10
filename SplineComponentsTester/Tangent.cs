using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace SplineComponentsTester
{
    public class Tangent : Control
    {
        // The velocity of this tangent
        private Vector2 m_Velocity;

        public Vector2 Velocity
        {
            get
            {
                // Return this tangents velocity
                return m_Velocity;
            }

            set
            {
                // Set this tangents velocity
                m_Velocity = value;

                // Call on change velocity event

            }
        }

        public Tangent()
        {

        }

        public Tangent(Vector2 a_Velocity)
        {
            m_Velocity = a_Velocity;
        }

        public static implicit operator Vector2(Tangent t)
        {
            return t.Velocity;
        }
    }
}
