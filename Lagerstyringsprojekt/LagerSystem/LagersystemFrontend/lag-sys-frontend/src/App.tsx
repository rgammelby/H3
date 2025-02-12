import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import HomePage from './HomePage';  // Import the HomePage component
import CreateDevicePage from './CreateDevicePage';  // Import CreateDevicePage component

function App() {
  return (
    <Router>
      <nav>
        <Link to="/">Home</Link> | <Link to="/create-device">Create Device</Link>
      </nav>

      <Routes>
        {/* Home Page Route */}
        <Route path="/" element={<HomePage />} />

        {/* Create Device Page Route */}
        <Route path="/create-device" element={<CreateDevicePage />} />
      </Routes>
    </Router>
  );
}

export default App;
