import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { Home } from "@/components/Home";
import { TelaDeLogin } from "@/components/TelaDeLogin";

function App() {
  return (
    <BrowserRouter>
      <nav className="border-b-4 border-zinc-950 bg-white px-4 py-3 sm:px-8">
        <div className="mx-auto flex max-w-7xl items-center justify-between gap-3">
          <Link
            to="/"
            className="text-3xl font-black tracking-[-0.08em] sm:text-3xl"
          >
            Piu
          </Link>
          <div className="flex items-center gap-2 text-sm font-bold uppercase sm:gap-4 sm:text-base">
            <Link
              to="/"
              className="rounded-lg px-3 py-2 hover:bg-zinc-100 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-zinc-950"
            >
              Home
            </Link>
            <Link
              to="/entrar"
              className="rounded-lg px-3 py-2 hover:bg-zinc-100 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-zinc-950"
            >
              Login
            </Link>
          </div>
        </div>
      </nav>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/entrar" element={<TelaDeLogin />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
