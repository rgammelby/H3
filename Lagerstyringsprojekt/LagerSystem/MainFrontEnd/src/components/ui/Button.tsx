// cva: A utility from class-variance-authority to manage class variants in Tailwind.
// VariantProps: A type helper for cva, making TypeScript recognize our button variants.
// ComponentProps: A type helper from React to get the props of a component.
import { cva, VariantProps } from "class-variance-authority";
import { ComponentProps } from "react";

// Creates a class generator function using cva 
// with base styles (transition-colors for smooth hover/focus effects).
const buttonStyles = cva(["transition-colors"],
  {
    variants:{
      variant:{
        default: ["bg-blue-500", "text-white", "hover:bg-blue-600"], // Force blue color
        ghost: ["bg-transparent", "border", "border-gray-400", "text-gray-600", "hover:bg-gray-200"]
      },
      size: {
        default:["rounded", "p-2"],
        icon: [
          "rounded-full", 
          "w-10", 
          "h-10", 
          "flex",
          "items-center",
          "justify-center",
          "p-2.5",
        ],
      },
    },
    // If no variant is provided, "default" style is applied.
    defaultVariants:{
      variant: "default",
      size: "default",
    }
  }
)

type ButtonProps = VariantProps<typeof buttonStyles> & ComponentProps<"button">

// Combines cva variants and React button props into a single type.
export function Button( { variant, size, ...props}: ButtonProps) {
  return <button {...props} className={buttonStyles({variant, size})} />
}
