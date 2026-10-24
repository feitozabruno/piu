import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Home } from "@/features/lotes/Home";
import { TelaDeLogin } from "@/features/auth/TelaDeLogin";
import { MenuNavegacao } from "@/shared/components/MenuNavegacao";

function App() {
  return (
    <BrowserRouter>
      <MenuNavegacao />

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/entrar" element={<TelaDeLogin />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
