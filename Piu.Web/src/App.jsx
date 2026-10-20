import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { Home } from "@/components/Home";
import { TelaDeLogin } from "@/components/TelaDeLogin";

function App() {
  return (
    <BrowserRouter>
      <nav>
        <Link to="/">Home</Link> | <Link to="/entrar">Login</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/entrar" element={<TelaDeLogin />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
