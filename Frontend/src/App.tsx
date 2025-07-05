import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import ProjectsPage from './Pages/ProjectsPage';
import EmployeesPage from './Pages/EmployeesPage';
import ProfilePage from './Pages/ProfilePage';
import LoginPage from './Pages/LoginPage';
import Layout from './Components/Layout/Layout';
import { AuthProvider, useAuth } from './Context/AuthContext';
import './Styles/Layout.css';
import '@fortawesome/fontawesome-free/css/all.min.css';
import TicketsPage from './Pages/TicketsPage';
import CalendarPage from './Pages/CalendarPage';
import { DataProvider } from './Context/DataContext';
import StatisticsPage from './Pages/StatisticsPage';

const App: React.FC = () => {
  return (
    <DataProvider>
      <AuthProvider>
        <Router>
          <Layout>
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route path="/profile" element={<PrivateRoute><ProfilePage /></PrivateRoute>} />
              <Route path="/projects" element={<PrivateRoute><ProjectsPage /></PrivateRoute>} />
              <Route path="/employees" element={<PrivateRoute><EmployeesPage /></PrivateRoute>} />
              <Route path="/tickets" element={<PrivateRoute><TicketsPage /></PrivateRoute>} />
              <Route path="/calendar" element={<PrivateRoute><CalendarPage /></PrivateRoute>} />
              <Route path="/statistics" element={<PrivateRoute><StatisticsPage /></PrivateRoute>} />             
              <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
          </Layout>
        </Router>
      </AuthProvider>
    </DataProvider>
  );
};

export default App;

const PrivateRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { isLoggedIn } = useAuth();
  return isLoggedIn ? <>{children}</> : <Navigate to="/login" />;
};