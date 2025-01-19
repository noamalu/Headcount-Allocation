import Sidebar from './Components/Layout/Sidebar';
import Header from './Components/Layout/Header';
import './Styles/App.css'
import '@fortawesome/fontawesome-free/css/all.min.css';
import ProjectsPage from './Pages/ProjectsPage';
// import './index.css';

const App = () => {
  return (
    <div className="app-container">
      <Sidebar />
      <Header />
      <main>
        <ProjectsPage />
      </main>
    </div>
  );
};

export default App;