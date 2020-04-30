import React from "react";
import {Menu} from "antd";
import {Link} from "react-router-dom";

import CustomScrollbars from "util/CustomScrollbars";
import SidebarLogo from "./SidebarLogo";
import UserProfile from "./UserProfile";
import AppsNavigation from "./AppsNavigation";
import {
  NAV_STYLE_NO_HEADER_EXPANDED_SIDEBAR,
  NAV_STYLE_NO_HEADER_MINI_SIDEBAR,
  THEME_TYPE_LITE
} from "../../constants/ThemeSetting";
import IntlMessages from "../../util/IntlMessages";
import {useSelector} from "react-redux";

const SubMenu = Menu.SubMenu;
const MenuItemGroup = Menu.ItemGroup;


const SidebarContent = () => {

  let {navStyle, themeType, pathname} = useSelector(({settings}) => settings);

  const getNoHeaderClass = (navStyle) => {
    if (navStyle === NAV_STYLE_NO_HEADER_MINI_SIDEBAR || navStyle === NAV_STYLE_NO_HEADER_EXPANDED_SIDEBAR) {
      return "gx-no-header-notifications";
    }
    return "";
  };
  const getNavStyleSubMenuClass = (navStyle) => {
    if (navStyle === NAV_STYLE_NO_HEADER_MINI_SIDEBAR) {
      return "gx-no-header-submenu-popup";
    }
    return "";
  };
  const selectedKeys = pathname.substr(1);
  const defaultOpenKeys = selectedKeys.split('/')[1];
  return (
    <>
      <SidebarLogo/>
      <div className="gx-sidebar-content">
        <div className={`gx-sidebar-notifications ${getNoHeaderClass(navStyle)}`}>
          <UserProfile/>
          <AppsNavigation/>
        </div>
        <CustomScrollbars className="gx-layout-sider-scrollbar">
          <Menu
            defaultOpenKeys={[defaultOpenKeys]}
            selectedKeys={[selectedKeys]}
            theme={themeType === THEME_TYPE_LITE ? 'lite' : 'dark'}
            mode="inline">

            {/* Tokenize BEGIN */}
            <MenuItemGroup key="tokenize" className="gx-menu-group" title={<IntlMessages id="sidebar.tokenize"/>}>
              <Menu.Item key="tokenize/dashboard">
                <Link to="/tokenize/dashboard"><i className="icon icon-dasbhoard"/>
                  <span><IntlMessages id="sidebar.dashboard"/></span></Link>
              </Menu.Item>

              <Menu.Item key="tokenize/tokens">
                <Link to="/tokenize/tokens"><i className="icon icon-apps"/>
                  <span><IntlMessages id="sidebar.tokens"/></span></Link>
              </Menu.Item>

              <Menu.Item key="tokenize/login">
                <Link to="/tokenize/login"><i className="icon icon-card"/>
                 <span> <IntlMessages id="sidebar.login"/></span></Link>
              </Menu.Item>

            </MenuItemGroup>
            {/* Tokenize END */}
          </Menu>
        </CustomScrollbars>
      </div>
    </>
  );
};

SidebarContent.propTypes = {};
export default SidebarContent;

