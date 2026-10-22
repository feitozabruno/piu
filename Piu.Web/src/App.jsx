import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Home } from "@/components/Home";
import { MenuNavegacao } from "@/components/MenuNavegacao";
import { TelaDeLogin } from "@/components/TelaDeLogin";

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
