import {NavLink, useLocation } from 'react-router-dom';
import {HomeOutlined, LoginOutlined} from '@ant-design/icons';
import { Menu } from 'antd';
import { ReactNode} from 'react';


type menuItem = {
  path: string,
  label: string,
  icon: ReactNode;
}


function Navigation() {
  const activePage = useLocation();

  const menuItems: Array<menuItem> = [
    {path: '/', label:'StartSeite', icon: <HomeOutlined/>},
    {path: '/login', label:'Anemelden', icon: <LoginOutlined/>},
  ]
  const selectedMenuItem = menuItems.find(item => item.path === activePage.pathname) ?? {path: '/', label:'StartSeite', icon: <HomeOutlined/>};

   return  (
      <Menu theme="dark" selectedKeys={[selectedMenuItem?.path]} > 
        { menuItems.map(menuItem => (
          <Menu.Item key={menuItem.path} icon={menuItem.icon}>
            <NavLink  to={menuItem.path}> {menuItem.label}</NavLink> 
          </Menu.Item>
        ))}
      </Menu>
    ) 
}
export default Navigation