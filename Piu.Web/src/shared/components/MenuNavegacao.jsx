import { Boxes, FileText, House, LogIn, LogOut } from "lucide-react";
import { NavLink, useLocation, useNavigate } from "react-router-dom";
import { Button } from "@/shared/components/ui/button";
import { useAuth } from "@/features/auth/useAuth";

const homeNavigationItem = { to: "/", label: "Home", icon: House };
const reportsNavigationItem = {
  to: "/relatorios",
  label: "Relatórios",
  icon: FileText,
};
const lotsNavigationItem = { to: "/lotes", label: "Lotes", icon: Boxes };
const loginNavigationItem = { to: "/entrar", label: "Login", icon: LogIn };

export function MenuNavegacao() {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const { isAuthenticated, error, logout } = useAuth(pathname);

  if (pathname === "/entrar") {
    return null;
  }

  const navigationItems = [
    homeNavigationItem,
    ...(isAuthenticated === true
      ? [lotsNavigationItem, reportsNavigationItem]
      : []),
    ...(isAuthenticated === false ? [loginNavigationItem] : []),
  ];

  async function handleLogout() {
    const loggedOut = await logout();

    if (loggedOut) {
      navigate("/entrar", { replace: true });
    }
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
          {isAuthenticated === true && (
            <Button
              type="button"
              onClick={handleLogout}
              aria-label="Sair da conta"
              title={error ?? "Sair da conta"}
              className="min-h-10 rounded-xl bg-transparent px-3 text-xs font-black uppercase tracking-[0.06em] text-zinc-600 shadow-none hover:bg-white hover:text-zinc-950 focus-visible:outline-4 focus-visible:outline-[#f6c945] sm:px-4 sm:text-sm"
            >
              <LogOut aria-hidden="true" className="size-4" strokeWidth={2.5} />
              Sair
            </Button>
          )}
        </div>
      </div>
    </nav>
  );
}
