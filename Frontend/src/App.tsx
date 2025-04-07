// src/App.tsx
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

const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/profile" element={<PrivateRoute><ProfilePage /></PrivateRoute>} />
            <Route path="/projects" element={<PrivateRoute><ProjectsPage /></PrivateRoute>} />
            <Route path="/employees" element={<PrivateRoute><EmployeesPage /></PrivateRoute>} />
            <Route path="/tickets" element={<PrivateRoute><TicketsPage /></PrivateRoute>} />
            <Route path="*" element={<Navigate to="/login" />} />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
};

export default App;

const PrivateRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { isLoggedIn } = useAuth();
  return isLoggedIn ? <>{children}</> : <Navigate to="/login" />;
};


// // src/App.tsx
// import React from 'react';
// import { BrowserRouter as Router, Routes, Route, Navigate, useLocation } from 'react-router-dom';
// import Sidebar from './Components/Layout/Sidebar';
// import Header from './Components/Layout/Header';
// import ProjectsPage from './Pages/ProjectsPage';
// import ProfilePage from './Pages/ProfilePage';
// import LoginPage from './Pages/LoginPage';
// import { AuthProvider, useAuth } from './Context/AuthContext';
// import './Styles/Layout.css';
// import '@fortawesome/fontawesome-free/css/all.min.css';

// const App: React.FC = () => {
//   return (
//     <AuthProvider>
//       <Router>
//         <MainLayout>
//           <Routes>
//             <Route path="/login" element={<LoginPage />} />
//             <Route path="/profile" element={<PrivateRoute><ProfilePage /></PrivateRoute>} />
//             <Route path="/projects" element={<PrivateRoute><ProjectsPage /></PrivateRoute>} />
//             <Route path="*" element={<Navigate to="/login" />} />
//           </Routes>
//         </MainLayout>
//       </Router>
//     </AuthProvider>
//   );
// };

// export default App;

// // ✅ רכיב עזר להגנה על מסכים – רק אם מחובר
// const PrivateRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
//   const { isLoggedIn } = useAuth();
//   return isLoggedIn ? <>{children}</> : <Navigate to="/login" />;
// };

// // ✅ תצוגה של Layout (Sidebar+Header) רק כשהמשתמש מחובר
// const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
//   const { isLoggedIn } = useAuth();
//   const location = useLocation();
//   const isLoginPage = location.pathname === '/login';

//   return (
//     <div className={`app-container ${isLoginPage ? 'login-layout' : ''}`}>
//       {isLoggedIn && !isLoginPage && <Sidebar />}
//         {isLoggedIn && !isLoginPage && <Header />}
//         {/* 💡 לא נעטוף ב-main אם זה מסך login */}
//         {isLoginPage ? children : <main>{children}</main>}
//     </div>
//   );
// };