import { cn } from "@/lib/cn";
import { Loader2 } from "lucide-react";

type ButtonVariant = "primary" | "secondary" | "ghost" | "outline" | "danger";
type ButtonSize = "sm" | "md" | "lg";

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  size?: ButtonSize;
  loading?: boolean;
  children: React.ReactNode;
}

const variants: Record<ButtonVariant, string> = {
  primary:
    "bg-accent text-bg-base font-semibold hover:bg-accent-bright shadow-glow-sm hover:shadow-glow-md border border-accent/50",
  secondary:
    "bg-bg-card text-text-primary border border-border-default hover:border-accent/40 hover:bg-bg-hover",
  ghost:
    "bg-transparent text-text-secondary hover:text-text-primary hover:bg-bg-hover border border-transparent",
  outline:
    "bg-transparent text-accent border border-accent/40 hover:bg-accent-dim hover:border-accent/60",
  danger:
    "bg-error-dim text-error border border-error/30 hover:bg-error/20",
};

const sizes: Record<ButtonSize, string> = {
  sm: "px-3 py-1.5 text-xs rounded-lg",
  md: "px-5 py-2.5 text-sm rounded-xl",
  lg: "px-8 py-3.5 text-base rounded-xl",
};

export default function Button({
  variant = "primary",
  size = "md",
  loading = false,
  disabled,
  className,
  children,
  ...props
}: ButtonProps) {
  return (
    <button
      disabled={disabled || loading}
      className={cn(
        "inline-flex items-center justify-center gap-2 transition-all duration-300",
        "focus:outline-none focus-visible:ring-2 focus-visible:ring-accent/50 focus-visible:ring-offset-2 focus-visible:ring-offset-bg-base",
        "disabled:opacity-50 disabled:cursor-not-allowed disabled:shadow-none",
        variants[variant],
        sizes[size],
        className
      )}
      {...props}
    >
      {loading && <Loader2 className="animate-spin" size={size === "sm" ? 14 : 18} />}
      {children}
    </button>
  );
}
