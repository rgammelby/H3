import logo from '../../assets/logo.png';
import { Menu, Bell, Search, User, Mail } from 'lucide-react';
import { useState } from "react";

export function PageHeader({ toggleSidebar }: { toggleSidebar: () => void }){
    
    return (
    <div className="flex gap-10 lg:gap-20 justify-between pt-2 mb-6 mx-4">
       {/* Left Section: Menu Button & Logo */}
        <div className="flex gap-4 items-center flex-shrink-0">
        
            {/* Menu Button */}
            {/*isAuthenticated && ( // Show menu button only when logged in
                    <button className="btn btn-ghost btn-square">
                        <Menu className="w-6 h-6" />
                    </button>
            )*/}

            {/* Menu Button - Toggles Sidebar */}
            <button className="btn btn-ghost btn-square" onClick={toggleSidebar}>
                        <Menu className="w-6 h-6" />
                    </button>
            <a href="/">
                <img src={logo} alt="logo" className="h-6" />
            </a>
        </div>

        <form className="md:flex hidden gap-4 flex-grow justify-center">
            <div className="flex flex-grow max-w-[600px]">
                <input
                    type="search"
                    placeholder="Custom Search"
                    className="flex-grow rounded-l-full border border-gray-300 shadow-inner shadow-
                    text-lg w-full py-1 px-4 focus:border-blue-500 outline-none"
                />
                <button className='btn bg-gray-200 border border-gray-300 rounded-r-full px=4 py=2 flex-shrink-0'>
                    <Search className="w-5 h-5"/>
                </button>
            </div>
            <button className='btn bg-gray-200 rounded-full'> 
                <Mail className="w-5 h-5"/>
            </button>
        </form>

        <div className='flex flex-shrink-0 gap-x-4 md:gap-x-6'>
            <button className='md:hidden'> <Search className="w-5 h-5"/> </button>
            <button className='md:hidden'> <Mail className="w-5 h-5"/> </button>
            <button> <Bell className="w-5 h-5"/> </button>
            <button > <User className="w-5 h-5"/> </button>
        </div>
        
    </div>
)}

export default PageHeader;