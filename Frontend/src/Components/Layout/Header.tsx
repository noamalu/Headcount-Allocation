import React from 'react';
import '../../Styles/Layout.css'

const Header: React.FC = () => {
    return (
        <div className="header">
            <nav>
                <div className="breadcrumb">Projects/</div> {/* נתיב ניווט */}
                <div className="icon-container">
                    <i className="fas fa-user"></i>
                    <i className="fas fa-cog"></i>
                    <i className="fas fa-bell"></i>
                </div>
            </nav>
        </div>
    );
};

export default Header;