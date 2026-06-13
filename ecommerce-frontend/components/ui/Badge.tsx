import { cn } from "@/lib/cn";

type BadgeVariant = "default" | "accent" | "success" | "warning" | "error" | "outline";

interface BadgeProps {
  children: React.ReactNode;
  variant?: BadgeVariant;
  className?: string;
}

const variants: Record<BadgeVariant, string> = {
  default: "bg-bg-hover text-text-secondary border-border-subtle",
  accent: "bg-accent-dim text-accent border-accent/30",
  success: "bg-accent-dim text-accent-bright border-accent/40",
  warning: "bg-warning/10 text-warning border-warning/30",
  error: "bg-error-dim text-error border-error/30",
  outline: "bg-transparent text-text-secondary border-border-default",
};

export default function Badge({ children, variant = "default", className }: BadgeProps) {
  return (
    <span
      className={cn(
        "inline-flex items-center gap-1 px-2.5 py-1 text-xs font-medium rounded-full border",
        variants[variant],
        className
      )}
    >
      {children}
    </span>
  );
}
