import Sidebar from './Components/Layout/Sidebar';
import Header from './Components/Layout/Header';
import ProjectsTable from './Components/Features/Projects/ProjectsTable';
import AddButton from './Components/Shared/AddButton';
import './Styles/App.css'
// import './index.css';

const App = () => {
  return (
    <div className="app-container">
      <Sidebar />
      <main>
        <Header />
        <ProjectsTable />
        <AddButton />
      </main>
    </div>
  );
};

export default App;