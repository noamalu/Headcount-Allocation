import Sidebar from './Components/Layout/Sidebar';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from './Components/Layout/Header';
import ProjectsTable from './Components/Features/Projects/ProjectsTable';
import AddButton from './Components/Shared/AddButton';
import './index.css';

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