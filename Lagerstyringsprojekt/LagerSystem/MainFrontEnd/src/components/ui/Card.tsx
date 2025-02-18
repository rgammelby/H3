import React from "react";

export const Card: React.FC<{ className?: string; children: React.ReactNode }> = ({ className, children }) => {
  return <div className={`border border-gray-300 rounded-lg shadow-md ${className}`}>{children}</div>;
};

export const CardContent: React.FC<{ className?: string; children: React.ReactNode }> = ({ className, children }) => {
  return <div className={`p-4 ${className}`}>{children}</div>;
};
