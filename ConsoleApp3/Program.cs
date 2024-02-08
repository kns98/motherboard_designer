using System;
using System.Collections.Generic;
using System.Text;

namespace MotherboardSvgGenerator
{
    abstract class Component
    {
        public string Id { get; set; }
        public (int Width, int Height) Size { get; set; }
        public (int X, int Y) Position { get; set; }
        public abstract string ToSvg();
    }

    class CpuSocket : Component
    {
        public override string ToSvg()
        {
            return $"<rect id=\"{Id}\" x=\"{Position.X}\" y=\"{Position.Y}\" width=\"{Size.Width}\" height=\"{Size.Height}\" style=\"fill:orange; stroke:black; stroke-width:1\" />";
        }
    }

    class RamSlot : Component
    {
        public override string ToSvg()
        {
            return $"<rect id=\"{Id}\" x=\"{Position.X}\" y=\"{Position.Y}\" width=\"{Size.Width}\" height=\"{Size.Height}\" style=\"fill:blue; stroke:black; stroke-width:1\" />";
        }
    }

    class PcieSlot : Component
    {
        public override string ToSvg()
        {
            return $"<rect id=\"{Id}\" x=\"{Position.X}\" y=\"{Position.Y}\" width=\"{Size.Width}\" height=\"{Size.Height}\" style=\"fill:green; stroke:black; stroke-width:1\" />";
        }
    }

    class CoolingSolution : Component
    {
        public string Type { get; set; }

        public override string ToSvg()
        {
            if (Type == "Fan")
            {
                return $"<circle id=\"{Id}\" cx=\"{Position.X + Size.Width / 2}\" cy=\"{Position.Y + Size.Height / 2}\" r=\"{Size.Width / 2}\" style=\"fill:lightblue; stroke:black; stroke-width:1\" />";
            }
            else if (Type == "Heatsink")
            {
                return $"<rect id=\"{Id}\" x=\"{Position.X}\" y=\"{Position.Y}\" width=\"{Size.Width}\" height=\"{Size.Height}\" style=\"fill:url(#heatsinkPattern); stroke:black; stroke-width:1\" />";
            }
            return string.Empty;
        }
    }

    class Motherboard
    {
        public List<Component> Components { get; set; } = new List<Component>();
        public (int Width, int Height) Dimensions { get; set; } = (610, 244);

        public string ToSvg()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<svg width=\"{Dimensions.Width}\" height=\"{Dimensions.Height}\" xmlns=\"http://www.w3.org/2000/svg\">");

            sb.AppendLine("<defs>");
            sb.AppendLine("  <pattern id=\"heatsinkPattern\" width=\"10\" height=\"10\" patternUnits=\"userSpaceOnUse\">");
            sb.AppendLine("    <rect width=\"10\" height=\"10\" fill=\"grey\"/>");
            sb.AppendLine("    <path d=\"M 0,10 L 10,0 M -1,1 L 1,-1 M 9,11 L 11,9\" stroke=\"black\" stroke-width=\"2\"/>");
            sb.AppendLine("  </pattern>");
            sb.AppendLine("</defs>");

            sb.AppendLine("<rect x=\"0\" y=\"0\" width=\"610\" height=\"244\" style=\"fill:lightgrey; stroke:black; stroke-width:2\" />");

            foreach (var component in Components)
            {
                sb.AppendLine(component.ToSvg());
            }

            sb.AppendLine("</svg>");
            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Motherboard motherboard = new Motherboard();
            motherboard.Components.Add(new CpuSocket { Id = "cpu", Position = (50, 50), Size = (50, 50) });
            motherboard.Components.Add(new RamSlot { Id = "ram1", Position = (150, 30), Size = (100, 20) });
            motherboard.Components.Add(new RamSlot { Id = "ram2", Position = (150, 70), Size = (100, 20) });
            motherboard.Components.Add(new PcieSlot { Id = "pcie1", Position = (300, 30), Size = (150, 20) });
            motherboard.Components.Add(new PcieSlot { Id = "pcie2", Position = (300, 70), Size = (150, 20) });
            motherboard.Components.Add(new CoolingSolution { Id = "fan1", Position = (200, 100), Size = (40, 40), Type = "Fan" });
            motherboard.Components.Add(new CoolingSolution { Id = "heatsink1", Position = (260, 100), Size = (60, 40), Type = "Heatsink" });

            string svgContent = motherboard.ToSvg();
            Console.WriteLine(svgContent);
            // Optionally, write svgContent to a .svg file
            System.IO.File.WriteAllText("motherboard.svg", svgContent);
        }
    }
}
