import { cn } from "@/lib/cn";

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
}

export default function Input({ label, error, className, id, ...props }: InputProps) {
  return (
    <div>
      {label && (
        <label htmlFor={id} className="block text-sm font-medium text-text-secondary mb-2">
          {label}
        </label>
      )}
      <input
        id={id}
        className={cn(
          "block w-full rounded-xl border border-border-default bg-bg-surface px-4 py-3",
          "text-text-primary placeholder-text-muted",
          "focus:border-accent/60 focus:outline-none focus:ring-2 focus:ring-accent/20",
          "transition-all duration-300 sm:text-sm",
          error && "border-error/50 focus:border-error/60 focus:ring-error/20",
          className
        )}
        {...props}
      />
      {error && <p className="mt-2 text-sm text-error">{error}</p>}
    </div>
  );
}
