module.exports = {
    extends: ['@commitlint/config-conventional'],
    rules: {
      'type-enum': [2, 'always', [
        'FEATURE', 
        'FIX',      
        'DOCS',     
        'REFACTOR', 
        'TEST',     
        'CI',       
        'HOTFIX',   
        'CHORE',    
        'RELEASE',  
        'SP',       
        'BUGREPORT',
        'MERGE',    
        'STYLES',
        'CONFIGURATION',
      ]],
      'subject-case': [2, 'always', 'sentence-case'], //NOTE: Ensures the subject starts with a capital letter
    },
  };
  