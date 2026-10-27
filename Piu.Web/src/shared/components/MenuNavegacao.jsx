import {
  Boxes,
  Egg,
  FileText,
  House,
  LogIn,
  LogOut,
  Menu,
  X,
} from "lucide-react";
import { useState } from "react";
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
const eggsNavigationItem = { to: "/ovos", label: "Ovos recebidos", icon: Egg };
const loginNavigationItem = { to: "/entrar", label: "Login", icon: LogIn };

export function MenuNavegacao() {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const { isAuthenticated, error, logout } = useAuth(pathname);
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  if (pathname === "/entrar") {
    return null;
  }

  const navigationItems = [
    homeNavigationItem,
    ...(isAuthenticated === true
      ? [lotsNavigationItem, eggsNavigationItem, reportsNavigationItem]
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
    <nav
      className="border-b-2 border-zinc-950 bg-white px-4 py-3 sm:px-8"
      aria-label="Navegação principal"
    >
      <div className="mx-auto flex max-w-7xl flex-wrap items-center justify-between gap-3 sm:px-8 lg:px-10">
        <NavLink
          to="/"
          aria-label="Ir para a página inicial"
          className="rounded-xl border-2 border-zinc-950 bg-[#f6c945] px-3 py-1 text-3xl font-black -tracking-widest shadow-[3px_3px_0_0_#18181b] transition-transform hover:-translate-y-px focus-visible:outline-4 focus-visible:outline-[#f6c945] focus-visible:outline-offset-2 active:translate-y-0 active:shadow-none"
        >
          Piu
        </NavLink>

        <Button
          type="button"
          variant="ghost"
          size="icon-lg"
          aria-label={isMenuOpen ? "Fechar menu" : "Abrir menu"}
          aria-expanded={isMenuOpen}
          aria-controls="menu-navegacao-opcoes"
          onClick={() => setIsMenuOpen((isOpen) => !isOpen)}
          className="rounded-xl border-2 border-zinc-950 text-zinc-950 hover:bg-[#f6c945] md:hidden"
        >
          {isMenuOpen ? (
            <X aria-hidden="true" strokeWidth={2.5} />
          ) : (
            <Menu aria-hidden="true" strokeWidth={2.5} />
          )}
        </Button>

        <div
          id="menu-navegacao-opcoes"
          className={`w-full basis-full flex-col gap-1 rounded-2xl border-2 border-zinc-950 bg-zinc-100 p-1 sm:gap-2 md:flex md:w-auto md:basis-auto md:flex-row ${
            isMenuOpen ? "flex" : "hidden"
          }`}
        >
          {navigationItems.map(({ to, label, icon: Icon }) => (
            <NavLink
              key={to}
              to={to}
              end={to === "/"}
              onClick={() => setIsMenuOpen(false)}
              className={({ isActive }) =>
                `flex min-h-10 items-center gap-2 rounded-xl px-3 text-xs font-black uppercase tracking-[0.06em] transition-colors focus-visible:outline-4 focus-visible:outline-[#f6c945] sm:px-4 sm:text-sm md:min-w-0 ${
                  isActive
                    ? "bg-zinc-950 text-white md:w-auto"
                    : "text-zinc-600 hover:bg-white hover:text-zinc-950 md:w-auto"
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
              aria-label="Sair da conta"
              title={error ?? "Sair da conta"}
              onClick={() => {
                setIsMenuOpen(false);
                void handleLogout();
              }}
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
