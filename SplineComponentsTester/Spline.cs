using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace SplineComponentsTester
{
    public class Spline
    {
        [Flags]
        private enum XmlFlags
        {
            In = 1 << 0,
            Out = 1 << 1,

            Both = In | Out
        }

        public class Knot
        {
            public Vector2 Point { get; private set; }
            public Vector2 In { get; private set; }
            public Vector2 Out { get; private set; }
            
            public Knot(Vector2 a_point, Vector2 a_in, Vector2 a_out)
            {
                Point = a_point;
                In = a_in;
                Out = a_out;
            }
        }

        // Knots of this spline
        public List<Knot> m_Knots;

        public Spline()
        {
            m_Knots = new List<Knot>();
        }

        public Vector2 Interpolate(float time)
        {
            // Clamp the time
            time = MathF.Clamp(0f, (float)Segments(), time);

            // Get time floored
            float Fl = MathF.Floor(time);

            // Get the segment being interpolated
            Knot St = m_Knots[(int)Fl];
            Knot En = m_Knots[(int)Fl + 1];

            // Interpolate spline and return the calculate value
            return Vector2.Hermite(St.Point, En.Point, St.Out, En.In, time - Fl);
        }

        public int Knots()
        {
            return m_Knots.Count;
        }

        public int Segments()
        {
            return m_Knots.Count - 1;
        }

        public static void Save(Spline spline, string path)
        {
            // Create the xml document
            XmlDocument doc = new XmlDocument();

            // Does this file already exist?
            if (File.Exists(path))
            {
                // Load the spline
                doc.Load(path);

                // Remove all elements from the document
                doc.RemoveAll();
            }

            // Create the decleration for this document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Get the current node of the document
            XmlNode root = doc.DocumentElement;

            // Set decleration before the root node
            doc.InsertBefore(dec, root);

            // Create the element to contain the entire spline, add it to the document
            XmlElement core = doc.CreateElement("Spline");
            core.SetAttribute("Knots", spline.Knots().ToString());
            doc.AppendChild(core);

            // Create element for first knot and add it to the spline
            core.AppendChild(ConvertToXml(doc, spline.m_Knots[0], 0, XmlFlags.Out));

            // Cycle through every knot in the spline
            for (int i = 1; i < spline.m_Knots.Count - 1; ++i)
            {
                // Create element for this knot and add it to the spline
                core.AppendChild(ConvertToXml(doc, spline.m_Knots[i], i, XmlFlags.Both));
            }

            // Add final knot to spline
            core.AppendChild(ConvertToXml(doc, spline.m_Knots[spline.Segments()], spline.Segments(), XmlFlags.In));

            // Save the document
            doc.Save(path);
        }

        public static Spline Load(string path)
        {
            // Make the new spline
            Spline sp = new Spline();

            // Create the xml document
            XmlDocument doc = new XmlDocument();

            // Assure the file path exists
            if (!File.Exists(path)) { throw new FileNotFoundException("Could not locate file path", path); }

            // Load the spline
            doc.Load(path);

            // Get the spline from the document
            XmlElement spline = (XmlElement)doc.SelectSingleNode("Spline");

            // Assure this file is a spline file
            if (spline == null) { throw new InvalidDataException(path + " does not contain spline"); }

            // Get the amount of knots from the documents
            int size = int.Parse(spline.Attributes["Knots"].Value);

            // Cycle through every knot in the spline
            for (int i = 0; i < size; ++i)
            {
                // Add this knot to the new spline
                sp.m_Knots.Add(ConvertToKnot(spline, i));
            }

            // Return the spline
            return sp;
        }

        private static XmlElement ConvertToXml(XmlDocument doc, Knot knot, int id, XmlFlags flag)
        {
            // Create the element from the document
            XmlElement ele = doc.CreateElement("k" + id.ToString());

            // Create the element to represent the point
            XmlElement poi = doc.CreateElement("Point");

            // Create and set elements to for the point
            XmlElement poi_x = doc.CreateElement("X");
            XmlElement poi_y = doc.CreateElement("Y");

            poi_x.InnerText = knot.Point.x.ToString();
            poi_y.InnerText = knot.Point.y.ToString();

            // Add points to the point element
            poi.AppendChild(poi_x);
            poi.AppendChild(poi_y);

            // Add point to element
            ele.AppendChild(poi);

            // Does knot support in tangents?
            if (flag.HasFlag(XmlFlags.In))
            {
                // Create the element to represent the in tangent
                XmlElement ta_in = doc.CreateElement("In");

                // Create and set elements to for the tangent
                XmlElement ta_in_x = doc.CreateElement("X");
                XmlElement ta_in_y = doc.CreateElement("Y");

                ta_in_x.InnerText = knot.In.x.ToString();
                ta_in_y.InnerText = knot.In.y.ToString();

                // Add points to the tangent element
                ta_in.AppendChild(ta_in_x);
                ta_in.AppendChild(ta_in_y);

                // Add tangent to element
                ele.AppendChild(ta_in);
            }

            // Does knot support in tangents?
            if (flag.HasFlag(XmlFlags.Out))
            {
                // Create the element to represent the out tangent
                XmlElement ta_out = doc.CreateElement("Out");

                // Create and set elements to for the tangent
                XmlElement ta_out_x = doc.CreateElement("X");
                XmlElement ta_out_y = doc.CreateElement("Y");

                ta_out_x.InnerText = knot.Out.x.ToString();
                ta_out_y.InnerText = knot.Out.y.ToString();

                // Add points to the tangent element
                ta_out.AppendChild(ta_out_x);
                ta_out.AppendChild(ta_out_y);

                // Add tangent to element
                ele.AppendChild(ta_out);
            }

            // Return the element
            return ele;
        }

        private static Knot ConvertToKnot(XmlElement ele, int id)
        {
            // Create vectors to capture variables
            Vector2 po = new Vector2(0f, 0f);
            Vector2 tin = new Vector2(0f, 0f);
            Vector2 tout = new Vector2(0f, 0f);

            // Get the knot element from the spline
            XmlElement kn = (XmlElement)ele.SelectSingleNode("k" + id.ToString());

            // Get the point from the knot
            XmlElement point = (XmlElement)kn.SelectSingleNode("Point");

            // Set point vector to points values
            po.x = float.Parse(point.SelectSingleNode("X").InnerText);
            po.y = float.Parse(point.SelectSingleNode("Y").InnerText);

            // Get the in tangent from the knot
            XmlElement intan = (XmlElement)kn.SelectSingleNode("In");

            // Is there an in tangent for this knot?
            if (intan != null)
            {
                // Set in tangents vector to tangents values
                tin.x = float.Parse(intan.SelectSingleNode("X").InnerText);
                tin.y = float.Parse(intan.SelectSingleNode("Y").InnerText);
            }

            // Get the out tangent from the knot
            XmlElement outtan = (XmlElement)kn.SelectSingleNode("Out");

            // Is there an in tangent for this knot?
            if (outtan != null)
            {
                // Set in tangents vector to tangents values
                tout.x = float.Parse(outtan.SelectSingleNode("X").InnerText);
                tout.y = float.Parse(outtan.SelectSingleNode("Y").InnerText);
            }

            // Create and return the new knot
            return new Knot(po, tin, tout);
        }
    }
}
