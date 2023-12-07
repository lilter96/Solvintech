import './Header.css';
import { observer } from 'mobx-react-lite';
import { authStore } from '../stores/authStore';

const Header: React.FC = observer(() => {
  const handleLogout = async () => {
    await authStore.logout();
  };

  return (
    <header className="header">
      <div className="header-content">
        <span className="application-name">Solvintech</span>
        <div className="right-content">
          {authStore.isLoggedIn && 
            <span className="user-email">{authStore!.currentUser!.email}</span>
          }
          {authStore.isLoggedIn && 
            <button onClick={handleLogout} className='logout-button'><span>Logout</span></button>
          }
        </div>
      </div>
    </header>
  );
});

export default Header;
