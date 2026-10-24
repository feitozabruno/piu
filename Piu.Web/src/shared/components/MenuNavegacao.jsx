import { House, LogIn } from "lucide-react";
import { NavLink, useLocation } from "react-router-dom";

const navigationItems = [
  { to: "/", label: "Home", icon: House },
  { to: "/entrar", label: "Login", icon: LogIn },
];

export function MenuNavegacao() {
  const { pathname } = useLocation();

  if (pathname === "/entrar") {
    return null;
  }

  return (
    <nav className="border-b-2 border-zinc-950 bg-white px-4 py-3 sm:px-8">
      <div className="mx-auto flex max-w-7xl sm:px-8 lg:px-10 items-center justify-between gap-4">
        <NavLink
          to="/"
          aria-label="Ir para a página inicial"
          className="rounded-xl border-2 border-zinc-950 bg-[#f6c945] px-3 py-1 text-3xl font-black -tracking-widest shadow-[3px_3px_0_0_#18181b] transition-transform hover:-translate-y-px focus-visible:outline-4 focus-visible:outline-[#f6c945] focus-visible:outline-offset-2 active:translate-y-0 active:shadow-none"
        >
          Piu
        </NavLink>

        <div
          className="flex items-center gap-1 rounded-2xl border-2 border-zinc-950 bg-zinc-100 p-1 sm:gap-2"
          aria-label="Navegação principal"
        >
          {navigationItems.map(({ to, label, icon: Icon }) => (
            <NavLink
              key={to}
              to={to}
              end={to === "/"}
              className={({ isActive }) =>
                `flex min-h-10 items-center gap-2 rounded-xl px-3 text-xs font-black uppercase tracking-[0.06em] transition-colors focus-visible:outline-4 focus-visible:outline-[#f6c945] sm:px-4 sm:text-sm ${
                  isActive
                    ? "bg-zinc-950 text-white"
                    : "text-zinc-600 hover:bg-white hover:text-zinc-950"
                }`
              }
            >
              <Icon aria-hidden="true" className="size-4" strokeWidth={2.5} />
              {label}
            </NavLink>
          ))}
        </div>
      </div>
    </nav>
  );
}
